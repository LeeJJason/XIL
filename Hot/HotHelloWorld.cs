using IL;
using wxb;
using UnityEngine;
using System.Collections.Generic;
#pragma warning disable 169
#pragma warning disable 649

namespace hot
{
    // AutoInitAndRelease，热更模块初始化后会自动调用Init,在卸载热更模块时会调用Release
    [AutoInitAndRelease]
    // Platform属性可以只在特定平台下才会生效，默认不加是全平台生效
    //[wxb.Platform(UnityEngine.RuntimePlatform.WindowsPlayer)]    
    // 默认替换HeloWorld类下的方法
    [ReplaceType(typeof(HelloWorld))] 
    public static class HotHelloWorld
    {
        // 在实际使用补丁方式热更时，经常遇到一些，只是需要在原有函数之前或之后添加一些代码的情况，
        // 这时，你可以通过Hotfix来执行原先代码
        // 类似以下定义的Hotfix字段，会自动赋值
        static Hotfix __Hotfix_Start; // 保存回调自身数据

        // 替换HelloWorld.Start接口
        [ReplaceFunction()]
        static void Start(HelloWorld world)
        {
            wxb.L.LogFormat("Hot Start(HelloWorld world)");
            RefType refType = new RefType((object)world);
            refType.GetField<List<string>>("onTexts").Add("HotHelloWorld Start");
            __Hotfix_Start.Invoke(world); // 调回HelloWorld.Start自身接口
        }

        // 替换HelloWorld.OnGUI接口
        [ReplaceFunction()]
        static void OnGUI(HelloWorld world)
        {
            UnityEngine.GUILayout.Label("这里是补丁里的逻辑处理！！！");

            RefType refType = new RefType((object)world);
            var texts = refType.GetField<List<string>>("onTexts");
            foreach (var ator in texts)
                UnityEngine.GUILayout.Label(ator);
        }

        // AutoInitAndRelease属性
        // 在热更初始化后自动调用
        public static void Init()
        {
            wxb.L.LogFormat("HotHelloWorld.Init by AutoInitAndRelease");
        }

        public static void Reg()
        {
            // 手动注册
            // 替换HelloWorld.TestFunc
            hotMgr.ReplaceFunc(typeof(HelloWorld), "TestFunc", typeof(HotHelloWorld).GetMethod("TestFunc", hotMgr.bindingFlags));

            // 替换HelloWorld.TestField
            hotMgr.ReplaceField(typeof(HelloWorld), "__Hotfix_TestField", typeof(HotHelloWorld).GetMethod("TestField", hotMgr.bindingFlags));
        }

        // 手动注册，替换了HelloWorld.TestFunc
        static void TestFunc(HelloWorld world)
        {
            wxb.L.Log("HotHelloWorld.TestFunc");
        }

        // 手动注册，替换了HelloWorld.TestField
        static void TestField(HelloWorld world)
        {
            wxb.L.Log("HotHelloWorld.TestField");
        }

        static Hotfix __Hotfix_get_GetValue = null; // 保存回调自身数据

        // 替换接口HelloWorld.getValue属性
        [ReplaceFunction()]
        static int get_GetValue(HelloWorld world)
        {
            int value = (int)__Hotfix_get_GetValue.Invoke(world); // 调回HelloWorld.getValue自身接口
            return value + 10000;
        }

        // 替换HelloWorld.Test接口,带out ref参数
        [ReplaceFunction()]
        static void Test(HelloWorld world, RefOutParam<int> refValue, RefOutParam<int> outValue)
        {
            refValue.value = 10000;
            outValue.value = 20000;
        }

        static Hotfix __Hotfix_TestX_0;
        static Hotfix __Hotfix_TestX_1;
        static Hotfix __Hotfix_TestX_2;
        static Hotfix __Hotfix_TestX_3;

        // 同名函数，替换HelloWorld.Test()
        [ReplaceFunction("__Hotfix_TestX_0")]
        static void TestX(HelloWorld world)
        {
            __Hotfix_TestX_0.Invoke(world);
        }

        // 同名函数，替换HelloWorld.Test(string name)
        [ReplaceFunction("__Hotfix_TestX_1")]
        static void TestX(HelloWorld world, string name)
        {
            __Hotfix_TestX_1.Invoke(world, name);
        }

        // 同名函数，替换HelloWorld.Test(int x, long name)
        [ReplaceFunction("__Hotfix_TestX_2")]
        static void TestX(HelloWorld world, int x, long name)
        {
            __Hotfix_TestX_2.Invoke(world, x, name);
        }

        // 同名函数，替换HelloWorld.Test(int x, string name)
        [ReplaceFunction("__Hotfix_TestX_3")]
        static void TestX(HelloWorld world, int x, string name)
        {
            __Hotfix_TestX_3.Invoke(world, x, name);
        }

        // 替换HelloWorld.TestXX静态方法
        [ReplaceFunction()]
        static void TestXX(int p1, int p2, int p3, int p4, string p5, string p6, string p7, string p8, Vector3 p9, Vector2 p10, Vector4 p11)
        {
            wxb.L.LogFormat("p1:{0}", p1);
            wxb.L.LogFormat("p2:{0}", p2);
            wxb.L.LogFormat("p3:{0}", p3);
            wxb.L.LogFormat("p4:{0}", p4);
            wxb.L.LogFormat("p5:{0}", p5);
            wxb.L.LogFormat("p6:{0}", p6);
            wxb.L.LogFormat("p7:{0}", p7);
            wxb.L.LogFormat("p8:{0}", p8);
            wxb.L.LogFormat("p9:{0}", p9);
            wxb.L.LogFormat("p10:{0}", p10);
            wxb.L.LogFormat("p11:{0}", p11);
        }
    }

    // 默认替换类型TestReplaceFunction下的接口
    [wxb.ReplaceType("TestReplaceFunction")]
    public class HotfixTestReplaceFunction
    {
        // 替换TestReplaceFunction.Test1接口
        [wxb.ReplaceFunction("TestReplaceFunction")]
        static void Test1(TestReplaceFunction obj)
        {

        }

        // 替换TestReplaceFunction.Test2接口
        [wxb.ReplaceFunction("__Hotfix_Test2")]
        static void Test2(TestReplaceFunction obj)
        {

        }

        // 替换TestReplaceFunction.Test3接口
        [wxb.ReplaceFunction(typeof(TestReplaceFunction))]
        static void Test3(TestReplaceFunction obj)
        {

        }

        // 替换TestReplaceFunction.Test4接口
        [wxb.ReplaceFunction(typeof(TestReplaceFunction), "__Hotfix_Test4")]
        static void Test4(TestReplaceFunction obj)
        {

        }

        // 替换TestReplaceFunction.Test5接口
        [wxb.ReplaceFunction("TestReplaceFunction", "__Hotfix_Test5")]
        static void Test5(TestReplaceFunction obj)
        {

        }
    }
}