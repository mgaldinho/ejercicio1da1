using System.Globalization;
using Dominio;
using Logica;

namespace Consola;

class Program
{
    static void Main(string[] args)
    {
        SistemaTurnos sistema = new SistemaTurnos();
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine();
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE TURNOS MÉDICOS ===");
            Console.WriteLine("1. Registrar turno");
            Console.WriteLine("2. Listar turnos del día");
            Console.WriteLine("3. Listar turnos por médico");
            Console.WriteLine("4. Atender próximo turno");
            Console.WriteLine("5. Ver historial");
            Console.WriteLine("6. Recaudación del día");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        RegistrarTurno(sistema);
                        break;
                    case "2":
                        ListarTurnosDelDia(sistema);
                        break;
                    case "3":
                        ListarTurnosPorMedico(sistema);
                        break;
                    case "4":
                        AtenderProximoTurno(sistema);
                        break;
                    case "5":
                        VerHistorial(sistema);
                        break;
                    case "6":
                        VerRecaudacion(sistema);
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }
    }

    static void RegistrarTurno(SistemaTurnos sistema)
    {
        Console.WriteLine();
        Console.WriteLine("Seleccione el tipo de turno:");
        Console.WriteLine("1. Consulta general");
        Console.WriteLine("2. Consulta de urgencia");
        Console.WriteLine("3. Telemedicina");
        Console.Write("Opción: ");
        string? tipo = Console.ReadLine();

        List<Medico> medicos = sistema.ObtenerMedicos();

        Console.WriteLine();
        Console.WriteLine("Médicos disponibles:");
        for (int i = 0; i < medicos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {medicos[i]}");
        }

        int indiceMedico = LeerEntero("Seleccione médico: ") - 1;

        if (indiceMedico < 0 || indiceMedico >= medicos.Count)
        {
            Console.WriteLine("Médico inválido.");
            return;
        }

        Medico medico = medicos[indiceMedico];

        Console.Write("Nombre del paciente: ");
        string nombre = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Console.WriteLine("El nombre no puede estar vacío.");
            return;
        }

        Console.Write("Apellido del paciente: ");
        string apellido = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(apellido))
        {
            Console.WriteLine("El apellido no puede estar vacío.");
            return;
        }

        DateTime fechaNacimiento = LeerFecha("Fecha de nacimiento (dd/MM/yyyy): ");

        Console.Write("Obra social (dejar vacío si no tiene): ");
        string? obraSocial = Console.ReadLine();

        DateTime fechaHora = LeerFechaHora("Fecha y hora del turno (dd/MM/yyyy HH:mm): ");

        Paciente paciente = new Paciente(nombre, apellido, fechaNacimiento, obraSocial);
        Turno turno;

        switch (tipo)
        {
            case "1":
                turno = new ConsultaGeneral(medico, paciente, fechaHora);
                break;

            case "2":
                Console.WriteLine("Prioridad: 1-Baja, 2-Media, 3-Alta");
                int prioridadIngresada = LeerEntero("Seleccione prioridad: ");

                if (prioridadIngresada < 1 || prioridadIngresada > 3)
                {
                    Console.WriteLine("Prioridad inválida.");
                    return;
                }

                turno = new ConsultaUrgencia(
                    medico,
                    paciente,
                    fechaHora,
                    (PrioridadUrgencia)prioridadIngresada
                );
                break;

            case "3":
                Console.Write("Ingrese link de videollamada: ");
                string link = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(link))
                {
                    Console.WriteLine("El link de videollamada no puede estar vacío.");
                    return;
                }

                turno = new ConsultaTelemedicina(medico, paciente, fechaHora, link);
                break;

            default:
                Console.WriteLine("Tipo de turno inválido.");
                return;
        }

        bool registrado = sistema.RegistrarTurno(turno, out string mensaje);
        Console.WriteLine(mensaje);

        if (registrado)
        {
            Console.WriteLine($"Costo final: ${turno.CalcularCosto():0.00}");
        }
    }

    static void ListarTurnosDelDia(SistemaTurnos sistema)
    {
        DateTime fecha = LeerFecha("Ingrese la fecha (dd/MM/yyyy): ");
        List<Turno> turnos = sistema.ListarTurnosDelDia(fecha);

        if (!turnos.Any())
        {
            Console.WriteLine("No hay turnos para ese día.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== TURNOS DEL DÍA ===");
        foreach (Turno turno in turnos)
        {
            Console.WriteLine(turno.ObtenerInformacion());
            Console.WriteLine("---");
        }
    }

    static void ListarTurnosPorMedico(SistemaTurnos sistema)
    {
        Console.Write("Ingrese el nombre del médico: ");
        string nombreMedico = Console.ReadLine() ?? "";

        List<Turno> turnos = sistema.ListarTurnosPorMedico(nombreMedico);

        if (!turnos.Any())
        {
            Console.WriteLine("No hay turnos pendientes para ese médico.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== TURNOS DEL MÉDICO ===");
        foreach (Turno turno in turnos)
        {
            Console.WriteLine(turno.ObtenerInformacion());
            Console.WriteLine("---");
        }
    }

    static void AtenderProximoTurno(SistemaTurnos sistema)
    {
        Turno? turno = sistema.AtenderProximoTurno();

        if (turno == null)
        {
            Console.WriteLine("No hay turnos pendientes.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Se atendió el siguiente turno:");
        Console.WriteLine(turno.ObtenerInformacion());
    }

    static void VerHistorial(SistemaTurnos sistema)
    {
        List<Turno> historial = sistema.VerHistorial();

        if (!historial.Any())
        {
            Console.WriteLine("No hay turnos atendidos todavía.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== HISTORIAL ===");
        foreach (Turno turno in historial)
        {
            Console.WriteLine(turno.ObtenerInformacion());
            Console.WriteLine("---");
        }
    }

    static void VerRecaudacion(SistemaTurnos sistema)
    {
        DateTime fecha = LeerFecha("Ingrese la fecha (dd/MM/yyyy): ");
        decimal total = sistema.ObtenerRecaudacionDelDia(fecha);
        Console.WriteLine($"Recaudación del día: ${total:0.00}");
    }

    static int LeerEntero(string mensaje)
    {
        int valor;
        do
        {
            Console.Write(mensaje);
        } while (!int.TryParse(Console.ReadLine(), out valor));

        return valor;
    }

    static DateTime LeerFecha(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();
            if (DateTime.TryParseExact(entrada, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                return fecha;
            }
            Console.WriteLine("Formato inválido. Use dd/MM/yyyy (ejemplo: 25/12/2024).");
        }
    }

    static DateTime LeerFechaHora(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();
            if (DateTime.TryParseExact(entrada, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHora))
            {
                return fechaHora;
            }
            Console.WriteLine("Formato inválido. Use dd/MM/yyyy HH:mm (ejemplo: 25/12/2024 14:30).");
        }
    }
}