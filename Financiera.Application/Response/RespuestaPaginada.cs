
namespace Financiera.Application.Response
{
    public class RespuestaPaginada<T>
    {
        public IEnumerable<T> Elementos { get; set; } = Enumerable.Empty<T>();
        public int TotalPagina { get; set; }
        public int NumeroPagina { get; set; }
        public int TotalElementos { get; set; }
        public int TamanoPagina { get; set; }

        public RespuestaPaginada(IEnumerable<T> elementos, int totalElementos, int numeroPagina, int tamanoPagina)
        {
            Elementos = elementos;
            TotalElementos = totalElementos;
            NumeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
            TamanoPagina = tamanoPagina < 1 ? 1 : tamanoPagina;
            TotalPagina = (int)Math.Ceiling(totalElementos / (double)tamanoPagina);
        }
    }
}
