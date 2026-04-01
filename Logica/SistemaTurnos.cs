using Dominio;

namespace Logica;

public class SistemaTurnos
{
    private readonly List<Turno> _turnosPendientes;
    private readonly List<Turno> _historial;
    private readonly List<Medico> _medicos;

    public SistemaTurnos()
    {
        _turnosPendientes = new List<Turno>();
        _historial = new List<Turno>();
        _medicos = new List<Medico>();

        CargarMedicosIniciales();
    }

    private void CargarMedicosIniciales()
    {
        _medicos.Add(new Medico("Dr. Pérez", "Clínica General", 2500));
        _medicos.Add(new Medico("Dra. Gómez", "Pediatría", 3000));
        _medicos.Add(new Medico("Dr. Rodríguez", "Cardiología", 4000));
    }

    public List<Medico> ObtenerMedicos()
    {
        return new List<Medico>(_medicos);
    }

    public bool RegistrarTurno(Turno turno, out string mensaje)
    {
        foreach (Turno t in _turnosPendientes)
        {
            if (t.Medico.Nombre == turno.Medico.Nombre && t.FechaHora == turno.FechaHora)
            {
                mensaje = "No se puede registrar el turno: ya existe un turno para ese médico en ese horario.";
                return false;
            }
        }

        _turnosPendientes.Add(turno);
        mensaje = "Turno registrado correctamente.";
        return true;
    }

    public List<Turno> ListarTurnosDelDia(DateTime fecha)
    {
        List<Turno> resultado = new List<Turno>();

        foreach (Turno turno in _turnosPendientes)
        {
            if (turno.FechaHora.Date == fecha.Date)
            {
                resultado.Add(turno);
            }
        }

        resultado.Sort(CompararPorFechaHora);
        return resultado;
    }

    public List<Turno> ListarTurnosPorMedico(string nombreMedico)
    {
        List<Turno> resultado = new List<Turno>();

        foreach (Turno turno in _turnosPendientes)
        {
            if (turno.Medico.Nombre.ToLower() == nombreMedico.ToLower())
            {
                resultado.Add(turno);
            }
        }

        resultado.Sort(CompararPorFechaHora);
        return resultado;
    }

    public Turno? AtenderProximoTurno()
    {
        if (_turnosPendientes.Count == 0)
        {
            return null;
        }

        ConsultaUrgencia? mejorUrgencia = null;

        foreach (Turno turno in _turnosPendientes)
        {
            if (turno is ConsultaUrgencia urgencia)
            {
                if (mejorUrgencia == null)
                {
                    mejorUrgencia = urgencia;
                }
                else if (urgencia.Prioridad > mejorUrgencia.Prioridad)
                {
                    mejorUrgencia = urgencia;
                }
                else if (urgencia.Prioridad == mejorUrgencia.Prioridad &&
                         urgencia.FechaRegistro < mejorUrgencia.FechaRegistro)
                {
                    mejorUrgencia = urgencia;
                }
            }
        }

        Turno proximo;

        if (mejorUrgencia != null)
        {
            proximo = mejorUrgencia;
        }
        else
        {
            proximo = _turnosPendientes[0];

            foreach (Turno turno in _turnosPendientes)
            {
                if (turno.FechaRegistro < proximo.FechaRegistro)
                {
                    proximo = turno;
                }
            }
        }

        proximo.MarcarAtendido();
        _turnosPendientes.Remove(proximo);
        _historial.Add(proximo);

        return proximo;
    }

    public List<Turno> VerHistorial()
    {
        return new List<Turno>(_historial);
    }

    public decimal ObtenerRecaudacionTotal()
    {
        decimal total = 0;

        foreach (Turno turno in _historial)
        {
            total += turno.CalcularCosto();
        }

        return total;
    }

    public decimal ObtenerRecaudacionDelDia(DateTime fecha)
    {
        decimal total = 0;

        foreach (Turno turno in _historial)
        {
            if (turno.FechaHora.Date == fecha.Date)
            {
                total += turno.CalcularCosto();
            }
        }

        return total;
    }

    private int CompararPorFechaHora(Turno a, Turno b)
    {
        return a.FechaHora.CompareTo(b.FechaHora);
    }
}