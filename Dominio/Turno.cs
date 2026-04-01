namespace Dominio;

public abstract class Turno
{
    public Medico Medico { get; }
    public Paciente Paciente { get; }
    public DateTime FechaHora { get; }
    public DateTime FechaRegistro { get; }
    public bool FueAtendido { get; private set; }

    protected Turno(Medico medico, Paciente paciente, DateTime fechaHora)
    {
        Medico = medico;
        Paciente = paciente;
        FechaHora = fechaHora;
        FechaRegistro = DateTime.Now;
        FueAtendido = false;
    }

    public virtual decimal CalcularCosto()
    {
        decimal costo = Medico.ValorConsultaBase;

        if (Paciente.TieneObraSocial)
        {
            costo *= 0.85m; // Descuento del 15% para pacientes con obra social
        }

        return costo;
    }

    public void MarcarAtendido()
    {
        FueAtendido = true;
    }

    public abstract string ObtenerTipo();
    
    public virtual string ObtenerInformacion()
    {
        return $"Fecha y Hora: {FechaHora:dd/MM/yyyy HH:mm}\n" +
               $"Turno: {ObtenerTipo()}\n" +
               $"Médico: {Medico.Nombre}\n" +
               $"Paciente: {Paciente.NombreCompleto}\n" +
               $"Costo: {CalcularCosto():0.00}";
    }
}