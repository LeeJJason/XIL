namespace wxb
{
    class UnityObjectSerialize : ITypeSerialize
    {
        public static byte type { get { return 20; } }

        byte ITypeSerialize.typeFlag { get { return type; } } // ���ͱ�ʶ

        int ITypeSerialize.CalculateSize(object value)
        {
            if (value == null)
                return 0;

            return 2;
        }

        void ITypeSerialize.WriteTo(object value, IStream stream)
        {
            if (value == null)
                return;

            stream.WriteUnityObject((UnityEngine.Object)value);
        }

        void ITypeSerialize.MergeFrom(ref object parent, IStream stream)
        {
            parent = stream.ReadUnityObject();
        }

        // ����ת��
        public static UnityEngine.Object To(UnityEngine.Object src, System.Type type)
        {
            if (src == null)
                return src;

#if USE_HOT
            if (type is ILRuntime.Reflection.ILRuntimeWrapperType)
                type = ((ILRuntime.Reflection.ILRuntimeWrapperType)type).RealType;
#endif
            var srcType = src.GetType();
            if (srcType == type)
                return src;

            if (type.IsAssignableFrom(srcType))
                return src; // src : type

            //if (typeof(UnityEngine.Component).IsAssignableFrom(type))
            //{
            //    if (src is UnityEngine.Component)
            //        return ((UnityEngine.Component)src).GetComponent(type);
            //    else if (src is UnityEngine.GameObject)
            //        return ((UnityEngine.GameObject)src).GetComponent(type);
            //}
            //else if (type == typeof(UnityEngine.GameObject))
            //{
            //    if (src is UnityEngine.Component)
            //        return ((UnityEngine.Component)src).gameObject;
            //}

            return null;
        }
    }
}