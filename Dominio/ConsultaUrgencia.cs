namespace Dominio;

public class ConsultaUrgencia : Turno
{
    public PrioridadUrgencia Prioridad { get; }
    
    public ConsultaUrgencia(Medico medico, Paciente paciente, DateTime fechaHora, PrioridadUrgencia prioridad) 
        : base(medico, paciente, fechaHora)
    {
        Prioridad = prioridad;
    }

    public override decimal CalcularCosto()
    {
        return base.CalcularCosto() * 1.5m;
    }

    public override string ObtenerTipo()
    {
        return "Consulta de Urgencia";
    }

    public override string ObtenerInformacion()
    {
        return base.ObtenerInformacion() + $"\nPrioridad: {Prioridad}";
    }
}