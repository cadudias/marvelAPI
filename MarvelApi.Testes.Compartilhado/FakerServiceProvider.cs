using System;
using System.Collections.Generic;

namespace MarvelApi.Testes.Compartilhado
{
    public static class FakerServiceProvider
    {
        private static Dictionary<Type, object> fakers = new Dictionary<Type, object>();

        public static void Add<T>(T faker) where T : class
        {
            Remover<T>();
            fakers.Add(typeof(T), faker);
        }

#pragma warning disable CS8603
        public static T Get<T>() where T : class => Contem<T>() ? (T)fakers[typeof(T)] : null;
#pragma warning restore CS8603

        public static bool Contem<T>() where T : class => fakers.ContainsKey(typeof(T));

        public static void Remover<T>() where T : class
        {
            if (Contem<T>())
                fakers.Remove(typeof(T));
        }

        public static void Clear() => fakers = new Dictionary<Type, object>();
    }
}