{/*
Se dispone de un sistema compuesto por 1 central y 2 procesos periféricos, que se comunican continuamente. Se requiere modelar su funcionamiento considerando las siguientes condiciones:
    - La central siempre comienza su ejecución tomando una señal del proceso 1; luego toma aleatoriamente señales de cualquiera de los dos indefinidamente. Al recibir una señal de proceso 2, recibe señales del mismo proceso durante 3 minutos.

    - Los procesos periféricos envían señales continuamente a la central. La señal del proceso 1 será considerada vieja (se deshecha) si en 2 minutos no fue recibida. Si la señal del proceso 2 no puede ser recibida inmediatamente, entonces espera 1 minuto y vuelve a mandarla (no se  deshecha).
*/}

Procedure Sistema is
//* especificación tasks */
    Task Central is
        Entry Senial1;
        Entry Senial2;
        Entry Tiempo;
    End Central;
    Task Periferico1;
    Task Periferico2;
    Task Reloj is
        Entry Iniciar;
    End Reloj;

//* cuerpo tasks */
    Task Body Reloj is
    Begin
        accept Iniciar();
        delay(180);
        Central.Tiempo();
    End Reloj;

    Task Body Periferico1 is
    Begin
        loop
            select
                Central.Senial1();
            or delay 120.0
                null
            end select;
        end loop;
    End Periferico1;

    Task Body Periferico2 is
    Begin
        loop 
            select 
                Central.Senial2();
            else 
                delay 60.0;
            end select;
        end loop;
    End Periferico2;

    Task Body Central is
        senial2: bool := false;
    Begin
        accept Senial1();
        loop
            select
                accept Senial1();
            or
                accept Senial2();
                senial2 := true;
                Reloj.Iniciar();
            end select;

            while(senial2) loop
                select
                    accept Tiempo();
                    senial2 := false;
                or 
                    when (Tiempo`count = 0) => accept Senial2(); 
                end select;
            end while;
        end loop;
    End Central;
Begin
    null;
End Sistema;