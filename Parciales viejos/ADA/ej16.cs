/*
Resolver con ADA el siguiente problema. Se debe controlar el acceso a una base de datos. Existen L procesos Lectores y E procesos Escritores que trabajan indefinidamente de la siguiente manera:
Escritor: intenta acceder para escribir, si no lo logra inmediatamente, espera 1 minuto y vuelve a intentarlo de la misma manera.
Lector: intenta acceder para leer, si no lo logro en 2 minutos, espera 5 minutos y vuelve a intentarlo de la misma manera.
Un proceso Escritor podrá acceder si no hay ningún otro proceso usando la base de datos; al acceder escribe y sale de la BD. Un proceso Lector podrá acceder si no hay procesos Escritores usando la base de datos; al acceder lee y sale de la BD. Siempre se le debe dar prioridad al pedido de acceso para escribir sobre el pedido de acceso para leer.

*/

Process
    Task type escritor;
    Task type lector;
    Task bd is
        entry accesoLector();
        entry accesoEscritor();
        entry salidaLector();
        entry salidaEscritor();
    end bd;
    //
    escritores: array(1..E) of escritor;
    lectores: array(1..L) of lector;
    //
    Task Body Lector is
    begin
        while true loop
            select bd.accesoLector() 
                // leer base de datos
                db.salidaLector();
            delay 120.0
                delay 300.0;
            end select;
        end loop;
    end Lector;
    Task Body escritor is
    begin
        while true loop
            select bd.accesoEscritor()
                // escribe base de datos
                db.salidaEscritor();
            else
                delay 60.0;
            end select;
        end loop
    end Escritor;
    Task body bd is
        hayL: int := 0;
    begin
        while true loop
            select 
                when (hayL == 0) --> 
                    accept accesoEscritor()
                    accept salidaEscritor();
                or (count`accesoEscritor == 0) --> 
                    accept accesoLector();
                    hayL := hayL + 1;
                or accept salidaLector()
                    hayL := hayL -1;
        end loop;
    end;
begin

end