# Plan de Pruebas – Sistema de Gestión de Turnos Médicos

## Información general

| Campo | Detalle |
|---|---|
| Solución | `Clinica.sln` |
| Proyectos | `Dominio`, `Logica`, `Consola` |
| Plataforma | .NET 8 |
| Fecha de ejecución | 01/04/2026 |

---

## Resultado de compilación

```
dotnet build Clinica.sln
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

✅ La solución compila sin errores ni advertencias.

---

## Médicos precargados

| # | Nombre | Especialidad | Valor base |
|---|---|---|---|
| 1 | Dr. Pérez | Clínica General | $2500 |
| 2 | Dra. Gómez | Pediatría | $3000 |
| 3 | Dr. Rodríguez | Cardiología | $4000 |

---

## Casos de prueba

### CP-01 – Registrar consulta general válida

| Campo | Valor |
|---|---|
| **Pasos** | Opción 1 → tipo 1 (General) → Dr. Pérez → Juan García → sin obra social → 01/04/2026 10:00 |
| **Resultado esperado** | Turno registrado. Costo: $2500.00 (base sin descuentos) |
| **Resultado observado** | `Turno registrado correctamente. Costo final: $2500.00` |
| **Estado** | ✅ PASA |

---

### CP-02 – Registrar urgencia y verificar recargo del 50%

| Campo | Valor |
|---|---|
| **Pasos** | Opción 1 → tipo 2 (Urgencia) → Dr. Pérez → sin obra social → prioridad Alta (3) |
| **Cálculo esperado** | $2500 × 1.5 = $3750.00 |
| **Resultado observado** | `Costo final: $3750.00` |
| **Estado** | ✅ PASA |

---

### CP-03 – Registrar telemedicina y verificar descuento del 20%

| Campo | Valor |
|---|---|
| **Pasos** | Opción 1 → tipo 3 (Telemedicina) → Dr. Rodríguez → sin obra social → link válido |
| **Cálculo esperado** | $4000 × 0.8 = $3200.00 |
| **Resultado observado** | `Costo final: $3200.00` |
| **Estado** | ✅ PASA |

---

### CP-04 – Paciente con obra social: descuento adicional del 15%

| Campo | Valor |
|---|---|
| **Pasos** | Opción 1 → tipo 1 (General) → Dr. Pérez → obra social: BPS |
| **Cálculo esperado** | $2500 × 0.85 = $2125.00 |
| **Resultado observado** | `Costo final: $2125.00` |
| **Estado** | ✅ PASA |

---

### CP-05 – Urgencia con obra social: recargo y descuento combinados

| Campo | Valor |
|---|---|
| **Pasos** | Urgencia Alta → Dr. Pérez → obra social: BPS |
| **Cálculo esperado** | $2500 × 0.85 × 1.5 = $3187.50 |
| **Resultado observado** | `Costo final: $3187.50` |
| **Estado** | ✅ PASA |

---

### CP-06 – Telemedicina con obra social: descuento telemedicina + obra social

| Campo | Valor |
|---|---|
| **Pasos** | Telemedicina → Dr. Rodríguez → obra social: BPS |
| **Cálculo esperado** | $4000 × 0.85 × 0.8 = $2720.00 |
| **Resultado observado** | `Costo final: $2720.00` |
| **Estado** | ✅ PASA |

---

### CP-07 – Impedir turno duplicado (mismo médico, misma fecha/hora)

| Campo | Valor |
|---|---|
| **Pasos** | Registrar turno para Dr. Pérez el 05/04/2026 10:00. Intentar registrar otro turno para Dr. Pérez en la misma fecha y hora. |
| **Resultado esperado** | El segundo intento es rechazado con mensaje de error. |
| **Resultado observado** | `No se puede registrar el turno: ya existe un turno para ese médico en ese horario.` |
| **Estado** | ✅ PASA |

---

### CP-08 – Permitir mismo horario con distinto médico

| Campo | Valor |
|---|---|
| **Pasos** | Registrar turno para Dr. Pérez el 05/04/2026 10:00. Registrar turno para Dra. Gómez en la misma fecha y hora. |
| **Resultado esperado** | Ambos turnos se registran correctamente. |
| **Resultado observado** | Ambos registrados. Segundo: `Turno registrado correctamente. Costo final: $3000.00` |
| **Estado** | ✅ PASA |

---

### CP-09 – Listar turnos del día ordenados por hora

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 3 turnos el 06/04/2026 en orden: 15:00, 09:00, 12:00. Listar turnos del día (opción 2). |
| **Resultado esperado** | Turnos mostrados en orden cronológico: 09:00 → 12:00 → 15:00 |
| **Resultado observado** | Dr. Rodríguez 09:00 → Dra. Gómez 12:00 → Dr. Pérez 15:00 |
| **Estado** | ✅ PASA |

---

### CP-10 – Listar turnos por médico

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 2 turnos para Dr. Pérez y 1 para Dra. Gómez. Listar por médico (opción 3): "Dr. Pérez". |
| **Resultado esperado** | Solo los 2 turnos de Dr. Pérez, ordenados por fecha/hora. |
| **Resultado observado** | Muestra exactamente los 2 turnos de Dr. Pérez (10:00 y 14:00). |
| **Estado** | ✅ PASA |

---

### CP-11 – Urgencias se atienden primero (por prioridad)

| Campo | Valor |
|---|---|
| **Pasos** | Registrar en orden: 1) ConsultaGeneral, 2) Urgencia Alta, 3) Urgencia Media. Atender 3 veces (opción 4). |
| **Resultado esperado** | Orden de atención: Urgencia Alta → Urgencia Media → ConsultaGeneral |
| **Resultado observado** | 1° atendida: Urgencia Alta. 2° atendida: Urgencia Media. 3° atendida: ConsultaGeneral. |
| **Estado** | ✅ PASA |

---

### CP-12 – Empate de prioridad: atender por orden de llegada (FIFO)

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 2 urgencias con la misma prioridad en orden de llegada. Atender (opción 4). |
| **Resultado esperado** | Se atiende primero la urgencia registrada antes. |
| **Resultado observado** | Urgencia Media registrada antes de otra Media es atendida primero. Se verifica que, ante urgencias con igual prioridad, se atiende primero la registrada antes. |
| **Estado** | ✅ PASA |

---

### CP-13 – Sin urgencias: atender por orden de llegada (FIFO)

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 2 ConsultasGenerales con distintos médicos. Atender 2 veces. |
| **Resultado esperado** | Se atiende primero el turno registrado primero, independientemente de la hora del turno. |
| **Resultado observado** | Dr. Pérez (registrado primero) atendido antes que Dra. Gómez. |
| **Estado** | ✅ PASA |

---

### CP-14 – Turnos atendidos pasan al historial

| Campo | Valor |
|---|---|
| **Pasos** | Registrar y atender 3 turnos. Ver historial (opción 5). |
| **Resultado esperado** | Historial muestra los 3 turnos atendidos en orden de atención. |
| **Resultado observado** | Historial muestra: Urgencia Alta, Urgencia Media, ConsultaGeneral (en ese orden). |
| **Estado** | ✅ PASA |

---

### CP-15 – Recaudación del día: solo con turnos atendidos

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 2 turnos (Dr. Pérez $2500 + Dr. Rodríguez $4000), atenderlos, consultar recaudación del día (opción 6). |
| **Resultado esperado** | $6500.00 |
| **Resultado observado** | `Recaudación del día: $6500.00` |
| **Estado** | ✅ PASA |

---

### CP-18 – Validación: fecha/hora en formato inválido

| Campo | Valor |
|---|---|
| **Pasos** | Al ingresar fecha/hora del turno, escribir "2026-04-12 10:00" (formato incorrecto), luego "bad-date", luego "12/04/2026 10:00" (correcto). |
| **Resultado esperado** | Los dos primeros intentos muestran error y vuelven a pedir. El tercero es aceptado. |
| **Resultado observado** | Muestra `Formato inválido. Use dd/MM/yyyy HH:mm (ejemplo: 25/12/2024 14:30).` en las dos primeras entradas. Acepta el tercer intento. |
| **Estado** | ✅ PASA |

---

### CP-19 – Validación: fecha de nacimiento en formato inválido

| Campo | Valor |
|---|---|
| **Pasos** | Al ingresar fecha de nacimiento, escribir "bad-date", luego "01/01/1980". |
| **Resultado esperado** | El primer intento muestra error. El segundo es aceptado. |
| **Resultado observado** | `Formato inválido. Use dd/MM/yyyy (ejemplo: 25/12/2024).` y luego acepta. |
| **Estado** | ✅ PASA |

---

### CP-20 – Validación: nombre del paciente vacío

| Campo | Valor |
|---|---|
| **Pasos** | Al pedir nombre del paciente, presionar Enter sin escribir nada. |
| **Resultado esperado** | Error y retorno al menú principal. |
| **Resultado observado** | `El nombre no puede estar vacío.` → vuelve al menú. |
| **Estado** | ✅ PASA |

---

### CP-21 – Validación: apellido del paciente vacío

| Campo | Valor |
|---|---|
| **Pasos** | Ingresar nombre válido, dejar apellido vacío. |
| **Resultado esperado** | Error y retorno al menú principal. |
| **Resultado observado** | `El apellido no puede estar vacío.` → vuelve al menú. |
| **Estado** | ✅ PASA |

---

### CP-22 – Validación: link de videollamada vacío (Telemedicina)

| Campo | Valor |
|---|---|
| **Pasos** | Tipo de turno 3 (Telemedicina). Al pedir link, presionar Enter sin escribir nada. |
| **Resultado esperado** | Error y retorno al menú principal. |
| **Resultado observado** | `El link de videollamada no puede estar vacío.` → vuelve al menú. |
| **Estado** | ✅ PASA |

---

### CP-23 – Validación: prioridad de urgencia fuera de rango (0 o 4)

| Campo | Valor |
|---|---|
| **Pasos** | Tipo de turno 2 (Urgencia). Ingresar prioridad 0. |
| **Resultado esperado** | Error y retorno al menú principal. |
| **Resultado observado** | `Prioridad inválida.` → vuelve al menú. |
| **Estado** | ✅ PASA |

---

### CP-24 – Validación: médico fuera de rango

| Campo | Valor |
|---|---|
| **Pasos** | Al seleccionar médico, ingresar 5 (solo hay 3 médicos). |
| **Resultado esperado** | Error y retorno al menú principal. |
| **Resultado observado** | `Médico inválido.` → vuelve al menú. |
| **Estado** | ✅ PASA |

---

### CP-25 – Atender cuando no hay turnos pendientes

| Campo | Valor |
|---|---|
| **Pasos** | Sin registrar ningún turno, seleccionar opción 4 (Atender próximo turno). |
| **Resultado esperado** | Mensaje informativo. |
| **Resultado observado** | `No hay turnos pendientes.` |
| **Estado** | ✅ PASA |

---

### CP-26 – Listar turnos del día cuando no hay resultados

| Campo | Valor |
|---|---|
| **Pasos** | Sin registrar ningún turno, seleccionar opción 2 (Listar turnos del día) e ingresar una fecha sin turnos registrados. |
| **Resultado esperado** | El sistema informa que no hay turnos para ese día. |
| **Resultado observado** | `No hay turnos para ese día.` |
| **Estado** | ✅ PASA |

---

### CP-27 – Listar turnos por médico sin resultados

| Campo | Valor |
|---|---|
| **Pasos** | Sin registrar ningún turno, seleccionar opción 3 (Listar turnos por médico) e ingresar el nombre de un médico que no tiene turnos pendientes. |
| **Resultado esperado** | El sistema informa que no hay turnos pendientes para ese médico. |
| **Resultado observado** | `No hay turnos pendientes para ese médico.` |
| **Estado** | ✅ PASA |

---

### CP-28 – Historial vacío antes de atender turnos

| Campo | Valor |
|---|---|
| **Pasos** | Sin atender ningún turno, seleccionar opción 5 (Ver historial). |
| **Resultado esperado** | El sistema informa que no hay turnos atendidos aún. |
| **Resultado observado** | `No hay turnos atendidos todavía.` |
| **Estado** | ✅ PASA |

---

### CP-29 – Entrada no numérica en selección de menú, médico y prioridad

| Campo | Valor |
|---|---|
| **Pasos** | a) Ingresar `abc` como opción de menú principal. b) Al seleccionar médico, ingresar `abc` y luego un número válido (1). c) Al seleccionar prioridad de urgencia, ingresar `abc` y luego un número válido (2). |
| **Resultado esperado** | a) El menú muestra "Opción inválida." y vuelve a mostrarse. b) `LeerEntero` repite el prompt hasta recibir un entero válido. c) Mismo comportamiento que (b) para la selección de prioridad. |
| **Resultado observado** | a) `Opción inválida.` → vuelve al menú. b) `Seleccione médico:` se repite hasta recibir un entero. c) `Seleccione prioridad:` se repite hasta recibir un entero. |
| **Estado** | ✅ PASA |

---

### CP-30 – Permitir mismo médico en distinta hora del mismo día

| Campo | Valor |
|---|---|
| **Pasos** | Registrar 3 turnos para Dr. Pérez el 28/04/2026 a las 09:00, 11:00 y 14:00 respectivamente. |
| **Resultado esperado** | Los 3 turnos se registran correctamente. Al listar el día se muestran los 3 en orden cronológico. |
| **Resultado observado** | Los 3 turnos registrados: `Turno registrado correctamente. Costo final: $2500.00` (×3). Listado del día muestra 09:00 → 11:00 → 14:00. |
| **Estado** | ✅ PASA |

---

## Resumen

| Total casos | Pasan | Fallan |
|---|---|---|
| 28 | 28 | 0 |

---

## Observaciones y notas de diseño

Las siguientes situaciones no son errores pero vale la pena documentarlas para revisión futura:

1. **`LeerEntero` no valida el rango**: si se ingresa un número entero pero fuera del rango esperado (por ejemplo, la selección de médico), la validación ocurre *después* de que la función retorna. Esto es aceptable pero la función podría recibir parámetros de rango min/max.

2. **Búsqueda por médico requiere nombre exacto**: `ListarTurnosPorMedico` compara en minúsculas, pero el usuario debe escribir el nombre completo (por ejemplo, `Dr. Pérez`). Si escribe `perez` sin el prefijo ni la tilde, no encontrará resultados. No es un bug, pero es un punto de usabilidad.

3. **La recaudación del día usa `FechaHora.Date`** (la fecha del turno), no la fecha en que fue atendido. Esto es coherente con la consigna pero vale documentarlo: si un turno del día anterior se atiende hoy, se cuenta en la recaudación del día anterior.

4. **No hay validación de que la fecha del turno sea futura**: es posible registrar turnos con fechas pasadas. La consigna no lo prohíbe explícitamente.

5. **Sin persistencia de datos**: al cerrar el programa se pierde toda la información. Esto es esperado en el alcance del ejercicio académico.
