int lectores, escritores = 0;
int esperaL, esperaE = 0;

sem mutex = 1;    // exclusión mutua para variables compartidas
sem semL, semE = 0;

Process Lector {
    P(e);
    if (escritores > 0) {
        esperaL++; V(e); P(semL);  // escritor activo → espera
    }
    lectores++;
    if (esperaL > 0 and lectores < 10 and esperaE == 0) { esperaL--; V(semL); }  // despierta otro lector
    else V(mutex);

    // lee la BD;

    P(mutex);
    lectores--;
    if (lectores == 0 && esperaE > 0) { 
        esperaE--; V(semE); }  // último lector → da paso a escritor
    else V(mutex);
}

Process Escritor {
    P(mutex);
    if (lectores > 0 || escritores > 0) {
        esperaE++; V(mutex); P(semE);  // lectores o escritor activo → espera
    }
    escritores++; V(mutex);

    //escribe la BD;

    P(mutex);
    escritores--;
    if (espera > 0) { esperaL--; V(semL); }  // da paso a lector
    else if (esperaE > 0) { esperaE--; V(semE); }  // o a otro escritor
    else V(mutex);
}