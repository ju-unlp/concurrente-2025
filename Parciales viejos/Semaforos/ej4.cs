/*
Resolver con SEMÁFOROS el siguiente problema. En un restorán trabajan C cocineros y M mozos. De forma repetida, los cocineros preparan un plato y lo dejan listo en la bandeja de platos terminados, mientras que los mozos toman los platos de esta bandeja para repartirlos entre los comensales. Tanto los cocineros como los mozos trabajan de a un plato por vez. Modele el funcionamiento del restorán considerando que la bandeja de platos listos puede almacenar hasta P platos. No es necesario modelar a los comensales ni que los procesos terminen.
*/

sem hayLugar = P
sem hayPlatos = 0;
sem mutexBandeja = 1;
queue bandeja;

Process Cocineros [id: 1..C]{
    plato p;
    while true{
        prepararPlato(p);
        // si hay lugar en la bandeja
        P(hayLugar);
        // deja plato
        P(mutexBandeja);
        push(bandeja, p);    
        V(mutexBandeja);
        // avisa a mozos
        V(hayPlatos);
    }
}

Process Mozos [id: 1..M]{
    plato p;
    while true{
        // si hay platos en bandeja
        P(hayPlatos);
        // saca plato
        P(mutexBandeja);
        pop(bandeja,p);
        V(mutexBandeja);
        // marca que hay lugar
        V(hayLugar);
        //
        entregarPlato(p);
    }
}

