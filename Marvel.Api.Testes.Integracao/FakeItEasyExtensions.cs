using System;
using FakeItEasy;
using FluentAssertions;

namespace MarvelApi.Testes
{
    public static class FakeItEasyExtensions
    {
        public static T BeEquivalentTo<T>(this IArgumentConstraintManager<T> comparacao, T valor)
        {
            var mensagem = string.Empty;
            return comparacao.Matches(a => BeEquivalentTo(a, valor, out mensagem), a => a.Write(mensagem));
        }

        private static bool BeEquivalentTo<TKey, TValue>(TKey v1, TValue v2, out string mensagem)
        {
            try
            {
                v1.Should().BeEquivalentTo(v2);
                mensagem = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                mensagem = e.Message;
                return false;
            }
        }
    }
}
