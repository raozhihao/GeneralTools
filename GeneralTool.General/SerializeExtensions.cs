namespace GeneralTool.General
{
    /// <summary>
    /// 序列化扩展类
    /// </summary>
    public static class SerializeExtensions
    {
        /// <summary>
        /// 序列化对象为字节数组,使用该方法后需要使用对应<see cref="Desrialize{T}(byte[])"/>反序列化回原始对象
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(this object obj)
        {
            return new SerializeHelpers().Serialize(obj);
        }

        /// <summary>
        /// 将字节数组转换回指定对象,使用该方法的前提是使用<see cref="Serialize(object)"/>方法转换的字节数组
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T Desrialize<T>(this byte[] bytes)
        {
            return new SerializeHelpers().Desrialize<T>(bytes);
        }

        /// <summary>
        /// 将字节数组转换回指定对象,使用该方法的前提是使用<see cref="Serialize(object)"/>方法转换的字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object Desrialize(this byte[] bytes)
        {
            return new SerializeHelpers().Desrialize(bytes);
        }
    }
}
