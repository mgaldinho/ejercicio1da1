namespace Dominio;

public class ConsultaGeneral : Turno
{
    public ConsultaGeneral(Medico medico, Paciente paciente, DateTime fechaHora) : base(medico, paciente, fechaHora)
    {
    }

    public override string ObtenerTipo()
    {
        return "Consulta General";
    }
}