{
En una playa hay 5 equipos de 4 personas cada uno (en total son 20 personas donde cada una conoce previamente a que equipo pertenece). Cuando las personas van llegando esperan con los de su equipo hasta que el mismo esté completo (hayan llegado los 4 integrantes), a partir de ese momento el equipo comienza a jugar. El juego consiste en que cada integrante del grupo junta 15 monedas de a una en una playa (las monedas pueden ser de 1, 2 o 5 pesos) y se suman los montos de las 60 monedas conseguidas en el grupo. Al finalizar cada persona debe conocer el grupo que más dinero junto. 
    Nota: maximizar la concurrencia. Suponga que para simular la búsqueda de una moneda por parte de una persona existe una función Moneda() que retorna el valor de la moneda encontrada.
}

// 5 equipos de 4 personas
// cada persona
    // sabe a que equipo pertenece previamente
    // cuando equipo completo
        // buscan monedas en playa
        // suman monto al EQUIPO
        // son 60 monedas

Procedure Equipo 
    { especificación tasks }
    Task Type Personas is
        entry IdentificarPersona(id: in integer; equipo: in integer);
        entry Empezar();
        entry Ganador(equipo: in integer);
    End Persona;

    Task Type Equipos is
        entry Identificar(id: in integer);
        entry Llegar(id: in integer);
        entry Puntaje(puntos: in integer);
    End Equipo;

    Task Playa is
        entry buscarMoneda(m: out moneda);
        entry totalEquipo(equipo: in integer; total in integer);
    End Playa;

    { variables typetask }
    arrPersonas: array (1..20) of Personas;
    arrEquipos: array (1..5) of Equipos;

    { cuerpo tasks }
    Task Body Equipos is
        id: integer;
        total: integer := 0;
        integrantes: array (1..4) of integer;
    begin
        // identificar
        accept Identificar(i) do
            id: i;
        end Identificar;
        // espera que lleguen integrantes
        for (i in 1..4) loop
            accept Llegar(id) do
                integrantes[i] := id;
            end Llegar;
        end loop;
        // los manda a buscar monedas
        for (i in 1..4) loop
            arrPersonas[integrantes[i]].Empezar();
        end loop;
        // espera que manden su valor
        for (i in 1..4) loop
            accept Puntaje(puntos) do
                total := total + puntos;
            end Puntaje;
        end loop;
        // manda valor total a la playa
        Playa.totalEquipo(id, total);
    end Equipos;
    ///////// persona
    Task Body Personas is
        id, equipo, valor, ganador: integer;
        m: moneda;
    begin
        // identificar
        accept IdentificarPersona(i, eq) do
            id:= i;
            equipo:= eq;
        end IdentificarPersona;
        // avisa que llego
        arrEquipos[equipo].Llegar(id);
        // espera que equipo le de ok
        accept Empezar();
        // busca monedas 15 veces
        for (i in 1..15) loop
            Playa.BuscarMoneda(m);
            valor := valor + Moneda(m);
        end loop;
        // mando a mi equipo mi puntaje
        arrEquipos[equipo].Puntaje(valor);
        // espero que me digan quien gano
        accept Ganador(ganador);
    end Personas;
    /////////// playa
    Task Body Playa is
        equiposTerminaron: integer := 0;
        max: integer := -1;
        ganador: integer;
    begin
        // mientras no terminen todos los equipos
        while (equiposTerminaron < 5) loop
            select
                accept BuscarMoneda(m) do
                    m := darMoneda();
                end BuscarMoneda;
            or 
                accept totalEquipo(id, total) do
                    equiposTerminaron := equiposTerminaron + 1;
                    if(total > max) then
                        max := total;
                        ganador := id;
                    end if;
                end TotalEquipo;
            end select;
        end loop;
        // le aviso a todos quien gano
        for (i in 1..20) loop
            arrPersonas[i].Ganador(ganador);
        end loop;
    end Playa;
begin
    for (i in 1..5) loop
        arrEquipos[i].Identificar(i);
    end loop;
    for (i in 1..20) loop
        arrPersonas[i].identificarPersona(i, ( (i mod 5) + 1 ));
    end loop;
end Equipo;