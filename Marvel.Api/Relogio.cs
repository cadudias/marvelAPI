using System;

namespace Marvel.Api
{
    public class Relogio
    {
        public virtual DateTime DataAtual => DateTime.Now;

        public virtual string ObterStringTicks() => DateTime.Now.Ticks.ToString();
    }
}