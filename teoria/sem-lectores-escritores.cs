int nr = 0;   // número de lectores activos
int nw = 0;   // número de escritores activos
int dr = 0;   // lectores en espera
int dw = 0;   // escritores en espera

sem e = 1;    // exclusión mutua para variables compartidas
sem r = 0;    // cola de espera para lectores
sem w = 0;    // cola de espera para escritores

Process Lector {
    P(e);
    if (nw > 0) {
        dr++; V(e); P(r);  // escritor activo → espera
    }
    nr++;
    if (dr > 0) { dr--; V(r); }  // despierta otro lector
    else V(e);

    // lee la BD;

    P(e);
    nr--;
    if (nr == 0 && dw > 0) { 
        dw--; V(w); }  // último lector → da paso a escritor
    else V(e);
}

Process Escritor {
    P(e);
    if (nr > 0 || nw > 0) {
        dw++; V(e); P(w);  // lectores o escritor activo → espera
    }
    nw++; V(e);

    //escribe la BD;

    P(e);
    nw--;
    if (dr > 0) { dr--; V(r); }  // da paso a lector
    else if (dw > 0) { dw--; V(w); }  // o a otro escritor
    else V(e);
}