Necesito asistencia para corregir una práctica de programación concurrente. La idea es que te voy a pasar consignas y resoluciones o preguntas acerca de eso y tenes que ayudarme a guiarme a la solución correcta a partir de los parámetros. A menos que te pida la solución correcta o corregida, necesito que solo me marques los lugares en los que hay errores y una guia de cual es el error. Estoy trabajando en pseudo-código. Actualmente estoy trabajando sobre monitores. las precondiciones son:

CONSIDERACIONES PARA RESOLVER LOS EJERCICIOS:
• Los monitores utilizan el protocolo signal and continue.
• A una variable condition SÓLO pueden aplicársele las operaciones SIGNAL,
SIGNALALL y WAIT.
• NO puede utilizarse el wait con prioridades.
• NO se puede utilizar ninguna operación que determine la cantidad de procesos
encolados en una variable condition o si está vacía.
• La única forma de comunicar datos entre monitores o entre un proceso y un monitor
es por medio de invocaciones al procedimiento del monitor del cual se quieren obtener
(o enviar) los datos.
• No existen variables globales.
• En todos los ejercicios debe maximizarse la concurrencia.
• En todos los ejercicios debe aprovecharse al máximo la característica de exclusión
mutua que brindan los monitores.
• Debe evitarse hacer busy waiting.
• En todos los ejercicios el tiempo debe representarse con la función delay.

Como ejemplo, este es un ejercicio resuelto de monitores:
"En una empresa de genética hay N clientes que envían secuencias de ADN para
que sean analizadas y esperan los resultados para poder continuar. Para resolver
estos análisis la empresa cuenta con un servidor que resuelve los pedidos de
acuerdo al orden de llegada de los mismos."

```
Process Cliente [id: 0..N-1]
{ text S, res;
while (true)
{ --generar secuencia S
Admin.Pedido(id, S, res);
}
}
Process Servidor
{ text sec, res;
int aux;
while (true)
{ Admin.Sig(aux, sec);
res = AnalizarSec(sec);
Admin.Resultado(aux, res);
}
}
Monitor Admin {
Cola C;
Cond espera;
Cond HayPedido;
text res[N];
Procedure Pedido (IdC: in int; S: in text; R: out text)
{ push (C, (idC,S) );
signal (HayPedido);
wait (espera);
R = res[idC];
}
Procedure Sig (IdC: out int; S: out text)
{ if (empty (C)) wait (HayPedido);
pop (C, (IdC, S));
}
Procedure Resultado (IdC: in int; R: in text)
{ res[IdC] = R;
signal (espera);
}
}
```

Otro ejemplo:
"En un entrenamiento de fútbol hay 20 jugadores que forman 4 equipos (cada jugador conoce el equipo al cual pertenece llamando a la función DarEquipo()). Cuando un equipo está listo (han llegado los 5 jugadores que lo componen), debe enfrentarse a otro equipo que también esté listo (los dos primeros equipos en juntarse juegan en la cancha 1, y los otros dos equipos juegan en la cancha 2). Una vez que el equipo conoce la cancha en la que juega, sus jugadores se dirigen a ella. Cuando los 10 jugadores del partido llegaron a la cancha comienza el partido, juegan durante 50 minutos, y al terminar todos los jugadores del partido se retiran (no es necesario que se esperen para salir)."
```
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
```

Analiza la información y respondeme con un resumen de las instrucciones que vas a seguir a lo largo de la conversacion