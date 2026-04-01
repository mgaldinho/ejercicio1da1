namespace Dominio;

public class ConsultaTelemedicina : Turno
{
    public string LinkVideoLlamada { get; }

    public ConsultaTelemedicina(Medico medico, Paciente paciente, DateTime fechaHora, string linkVideoLlamada)
        : base(medico, paciente, fechaHora)
    {
        LinkVideoLlamada = linkVideoLlamada;
    }

    public override decimal CalcularCosto()
    {
        return base.CalcularCosto() * 0.8m;
    }

    public override string ObtenerTipo()
    {
        return "Consulta por Telemedicina";
    }

    public override string ObtenerInformacion()
    {
        return base.ObtenerInformacion() + $"\n Link: {LinkVideoLlamada}";
    }
}