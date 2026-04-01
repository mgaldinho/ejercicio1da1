namespace Dominio;

public class Medico
{
    public string Nombre { get; }
    public string Especialidad { get; }
    public decimal ValorConsultaBase { get; }

    public Medico(string nombre, string especialidad, decimal valorConsultaBase)
    {
        Nombre = nombre;
        Especialidad = especialidad;
        ValorConsultaBase = valorConsultaBase;
    }

    public override string ToString()
    {
        return $"Medico: {Nombre}, Especialidad: {Especialidad}, Valor Consulta Base: {ValorConsultaBase}";
    }
}