/*
En un entrenamiento de fútbol hay 20 jugadores que forman 4 equipos (cada jugador conoce el equipo al cual pertenece llamando a la función DarEquipo()). Cuando un equipo está listo (han llegado los 5 jugadores que lo componen), debe enfrentarse a otro equipo que también esté listo (los dos primeros equipos en juntarse juegan en la cancha 1, y los otros dos equipos juegan en la cancha 2). Una vez que el equipo conoce la cancha en la que juega, sus jugadores se dirigen a ella. Cuando los 10 jugadores del partido llegaron a la cancha comienza el partido, juegan durante 50 minutos, y al terminar todos los jugadores del partido se retiran (no es necesario que se esperen para salir).
*/

Process Jugador[id: 0 to 19]{
    int equipo = darEquipo();
    int nCancha;

    Equipo[equipo].llegaJugador(nCancha);
    Cancha[nCancha].llegaJugador();
    
}

Process Partido[id: 0 to 1]{

    Cancha[id].iniciar();
    // jugar partido //
    delay(90 minutos);
    Cancha[id].terminar();
    
}

Monitor Admin{
    int equipos = 0;

    procedure asignarCancha(cancha: out int){
        equipos++;
        if(equipos < 2){
            cancha = 1;
        } else {
            cancha = 2;
        }
    }
}

Monitor Equipo[id: 0 to 3]{
    int total = 0;
    int cancha;
    cond jugadores;

    procedure llegaJugador(nroCancha: out int){
        // los jugadores llegan y se duermen hasta que estan completos
        total++;
        if(total == 5){
            signal_all(jugadores);
            Admin.asignarCancha(cancha);
        } else {
            wait(jugadores);
        }
        // lo ponemos acá asi todos los jugadores saben a que cancha ir
        nroCancha = cancha;
    }
}

Monitor Cancha[id: 0 to 1]{
    int total = 0;
    cond jugadores;
    cond partido;

    procedure llegaJugador(){
        total ++;
        if(total == 10){
            signal(partido);
        }
        wait(jugadores);
    }

    procedure iniciar(){
        if(total < 10){ 
            wait(partido); 
        }
    }

    procedure terminar(){
        signal_all(jugadores);
    }

}