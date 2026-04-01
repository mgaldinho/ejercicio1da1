namespace Dominio;

public class Paciente
{
    public string Nombre { get; }
    public string Apellido { get; }
    public DateTime FechaNacimiento { get; }
    public string? ObraSocial { get; }

    public Paciente(string nombre, string apellido, DateTime fechaNacimiento, string? obraSocial)
    {
        Nombre = nombre;
        Apellido = apellido;
        FechaNacimiento = fechaNacimiento;
        ObraSocial = string.IsNullOrWhiteSpace(obraSocial) ? null : obraSocial;
    }

    public bool TieneObraSocial => !string.IsNullOrEmpty(ObraSocial);
    
    public string NombreCompleto => $"{Nombre} {Apellido}";

    public override string ToString()
    {
        if (TieneObraSocial)
        {
            return $"{NombreCompleto} - Obra social: {ObraSocial}";
        }
        return $"{NombreCompleto} - Sin obra social";
    }
}