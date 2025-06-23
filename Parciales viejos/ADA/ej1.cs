/*
Resolver con ADA el siguiente problema. Se debe simular un juego en el que participan 30 jugadores que forman 5 grupos de 6 personas. Al llegar cada jugador debe buscar las instrucciones y el grupo al que pertenece en un cofre de cemento privado para cada uno; para esto deben usar un único martillo gigante de a uno a la vez y de acuerdo al orden de llegada. Luego se debe juntar con el resto de los integrantes de su grupo y los 6 juntos realizan las acciones que indican sus instrucciones. Cuando un grupo termina su juego le avisa a un Coordinador que le indica en qué orden término el grupo. 
Nota: maximizar la concurrencia; suponer que existe una función Jugar() que simula que los 6 integrantes de un grupo están jugando juntos; suponga que existe una función Romper(grupo) que simula cuando un jugador está rompiendo su cofre con el martillo y le retorna el grupo al que pertenece.
*/

// Solucion correcta //
// importante: el task que acepta una llamada con out, es necesario pasar la variable 

Procedure Juego
    TaskType Jugador;

    TaskType Grupo is
        entry Listo();
        entry PedirResultado(puesto: out integer);
    end  Grupo;

    Task Martillo is
        entry PedirMartillo(grupo: out int);
    end Martillo;

    Task Coordinador is
        entry TerminoGrupo(puesto: out integer);
    end Coordinador;

// declaración variables task
    arrJugador : array(1..30) of Jugador;
    arrGrupo : array(1..5) of Grupo;
// cuerpo tasks
    Task Body Jugador 
       g, puesto: int;
    begin
        Martillo.PedirMartillo(g);
        Grupo[g].Listo();
        Grupo[g].PedirResultado(puesto);  
    end Jugador;

    Task Body Grupo
        puesto: int;
    begin
        for int i 1..6 loop
            accept Listo();
        end loop;
        Jugar();
        Coordinador.TerminoGrupo(puesto);
        for int i 1..5 loop
            accept PedirResultado(p: out integer) do
                p := puesto;
            end PedirResultado;
        end loop;
    end Grupo;

    Task Body Martillo
        g: int;
    begin
        for int i 1..30 loop
            accept PedirMartillo(g: out integer) do
                Romper(g);
            end PedirMartillo;
        end loop
    end Martillo;

    Task Body Coordinador 
        pos: int;
    begin
        for int i 1..5 loop
            accept TerminoGrupo(pos: out integer) do
                pos := i;
            end TerminoGrupo;
        end loop
    end Coordinador;
Begin

End Procedure Juego;

///// SOLUCION PARCIALMENTE INCORRECTA ////
/// Problema de consiga: Martillo es su propio task
/// Problema consigna: el grupo le tiene que pasar a los jugadores el puesto
/// okay el Romper lo hace dentro de martillo

Procedure 
// especificación de tasks
    TaskType Jugador;

    TaskType Grupo is
        entry Listo();
    end  Grupo;

    Task Coordinador is
        entry PedirMartillo();
        entry DevolverMartillo();
        entry TerminoGrupo(puesto: out integer);
    end Coordinador;

// declaración variables task
    arrJugador : array(1..30) of Jugador;
    arrGrupo : array(1..5) of Grupo;
// cuerpo tasks
    Task Body Jugador 
       g: int;
    begin
        Coordinador.PedirMartillo();
        Romper(g);
        Coordinador.DevolverMartillo();
        Grupo[g].Listo();
    end Jugador;

    Task Body Grupo
        jugadores: int := 0;
        puesto: int;
    begin
        while( jugadores < 6){
            accept Listo();
        }
        Jugar();
        Coordinador.TerminoGrupo(puesto);
    end Grupo;

    Task Body Coordinador 
        martilloLibre: bool := true;
        ordenTermino: int := 1;
        grupo: int;
    begin
        loop
            select 
                when (martilloLibre) accept PedirMartillo() do
                    martilloLibre := false;
                end PedirMartillo; 
            or
                accept DevolverMartillo() do
                    martilloLibre := true;
                end DevolverMartillo;
            or 
                accept TerminoGrupo(ordenTermino);
                ordenTermino := ordenTermino = 1;
        end loop;
    end Coordinador;
Begin

End Procedure