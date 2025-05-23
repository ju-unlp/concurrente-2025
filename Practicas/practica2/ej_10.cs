/*
CHEQUEADO 

A una cerealera van T camiones a descargarse trigo y M camiones a descargar maíz. Sólo hay lugar para que 7 camiones a la vez descarguen, pero no pueden ser más de 5 del mismo tipo de cereal. Nota: no usar un proceso extra que actué como coordinador, resolverlo entre los camiones.
*/

sem lugarCamiones = 7;
sem espacioTrigo = 5, espacioMaiz = 5;

Process CamionTrigo[id: 0..T-1]
{
    while (true) {
        P(espacioTrigo);
        P(lugarCamiones);
        // descargar trigo
        V(lugarCamiones);
        V(espacioTrigo);
    }
}

Process CamionMaiz[id: 0..M-1]
{
    while (true) {
        P(espacioMaiz);
        P(lugarCamiones);
        // descargar maiz
        V(lugarCamiones);
        V(espacioMaiz);
    }
}