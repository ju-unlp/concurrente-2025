Necesito asistencia para corregir una práctica de programación concurrente. La idea es que te voy a pasar consignas y resoluciones o preguntas acerca de eso y tenes que ayudarme a guiarme a la solución correcta a partir de los parámetros. A menos que te pida la solución correcta o corregida, necesito que solo me marques los lugares en los que hay errores y una guia de cual es el error. Estoy trabajando en pseudo-código. Actualmente estoy trabajando sobre semáforos. las precondiciones son:

CONSIDERACIONES PARA RESOLVER LOS EJERCICIOS:
• Recuerde que en la práctica sólo se utilizan los semáforos GENERALES.
• Los semáforos deben estar declarados en todos los ejercicios.
• Los semáforos deben estar inicializados en todos los ejercicios.
• No se puede utilizar ninguna sentencia para setear o ver el valor de un semáforo.
• Debe evitarse hacer busy waiting en todos los ejercicios.
• En todos los ejercicios el tiempo debe representarse con la función delay.

EJERCICIOS DE EJEMPLO
En una montaña hay 30 escaladores que en una parte de la subida deben
utilizar un único paso de a uno a la vez y de acuerdo al orden de llegada al
mismo.
```
cola c;
sem mutex = 1, espera[30] = ([30] 0);
boolean libre = true;
Process Escalador[id: 0..29]
{ int aux;
-- llega al paso
P (mutex);
if (libre) { libre = false; V (mutex); }
else { push (C, id); V (mutex); P (espera[id]); };
//Usa el paso con Exclusión Mutua
P (mutex);
if (empty (C)) libre = true
else { pop (C, aux); V (espera[aux]); };
V (mutex);
}
```

En una empresa de genética hay N clientes que envían secuencias de ADN para
que sean analizadas y esperan los resultados para poder continuar. Para resolver
estos análisis la empresa cuenta con 2 servidores que van alternando su uso para
no exigirlos de más (en todo momento uno está trabajando y el otro
descansando); cada 5 horas cambia en servidor con el que se trabaja. El servidor
que está trabajando, toma un pedido (de a uno de acuerdo al orden de llegada de
los mismos), lo resuelve y devuelve el resultado al cliente correspondiente.
Cuando terminan las 5 horas se intercambian los servidores que atienden los
pedidos. Si al terminar las 5 horas el servidor se encuentre atendiendo un
pedido, lo termina y luego se intercambian los servidores.
```
sem mutex = 1, pedidos = 0, espera[N] = ([N] 0), inicio = 0, turno[2] = (1, 0);
int resultados[N]; cola C; bool finTiempo = false;

Process Cliente[id: 0..N-1]
{ secuencia S;
while (true)
{ --generar secuencia S
P(mutex);
push(C, (id, S));
V(mutex);
V(pedidos);
P(espera[id]);
--ver resultado de resultados[id]
}
}
Process Reloj
{ while (true)
{ P(inicio);
delay(5 hs);
finTiempo = true;
V(pedidos);
}
}
Process Servidor[id: 0..1]
{ secuencia sec; int aux; bool ok;
while (true)
{ P(turno[id]);
finTiempo = false;
V(inicio);
ok = true;
while (ok)
{ P(pedidos);
if (finTiempo) { ok = false;
V(turno[1-id]); }
else { P(mutex);
pop(C, (aux, sec));
V(mutex);
resultados[aux] = resolver(sec);
V(espera[aux]);
}
}
}
}
```