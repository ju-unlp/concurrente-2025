/*
Resolver con ADA la siguiente situación. En una oficina hay un empleado y P personas que van para ser atendidas para realizar un trámite. Cuando una persona llega espera a lo sumo 20 minutos a que comience a atenderla el empleado para resolver el trámite que va a hacer, y luego se va; si pasó el tiempo se retira sin realizar el trámite. El empleado atienden las solicitudes en orden de llegada. Cuando las P personas se han retirado el empelado también se retira. Nota: cada persona hace sólo un pedido y termina; suponga que existe una función Atender() llamada por el empleado que simula que el empleado está resolviendo el trámite del cliente; todas las tareas deben terminar.
*/

Procedure oficina
    task admin is
        entry retirada();
    end admin;
    task empleado is
        entry atencion(tramite: in string; resultado: out string);
        entry finDelDia();
    end empleado;
    task type persona;

    arrPersonas: array(1..P) of persona;

    task body admin
    begin
        for int i 1..P loop
            accept retirada();
        end loop;
        Empleado.finalDelDia();
    end admin;

    task body persona
        tramite, resultado: String;
    begin
        select
            Empleado.atencion(t, resultado);
        or delay 1200.0
            null
        end select;
        Admin.retirada();
    end persona;

    task body empleado
        finDia: bool := false;
        tramite, resultado: string;
    begin
        while (not finDia) loop
            select
                accept atencion(tramite, resultado: out string) do
                    resultado := hacerTramite(tramite);
                end atencion;
            or
                accept finDelDia();
                finDia:= true;
            end select;
        end loop;
    end empleado;
begin
    null;
end oficina;


////// sin terminar esta mal
/// no hace falta el identificar
/// hace falta un admin
/// 
Procedure oficina
// especific. tasks
    Task Empleado is
        entry llegada(id: in int);
    end Empleado;
    TaskType Persona is
        entry identificar(id: in int);
        entry turno(tramite: out string);
        entry finAtencion();
    end Persona;
// variables
arrPersonas: array(1..P) of Persona;
// cuerpo
    TaskBody Persona
        id: int;
        tramite: string;
        atendido: bool := true;
    begin
        accept identificar(id);
        Empleado.llegada(id);
        if accept turno(t: out string) do
                t := tramite;
            end turno;
        or delay 1200.0
            atendido := false;
    
    if (atendido) {
        accept finAtencion();
    }
    end Persona;

    //
    Task Body Empleado 

    begin 
        for int i 1..P loop

        end loop; 
    end Empleado;
begin

end oficina;