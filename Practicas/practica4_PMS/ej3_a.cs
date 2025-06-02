/*
En un examen final hay N alumnos y P profesores. Cada alumno resuelve su examen, lo entrega y espera a que alguno de los profesores lo corrija y le indique la nota. Los profesores corrigen los exámenes respetando el orden en que los alumnos van entregando.
    a. Implemente una solución con PMS considerando que P=1.

Nota: maximizar la concurrencia y no generar demora innecesaria.
*/

Process Alumno[id:0.. N-1]{
    tipo_examen exam;
    int nota;
    ResolverExamen(exam); // asumir que existe
    Profesor!entregar(id, exam);
    Profesor?corregir(nota);
}

Process Profesor{
    tipo_examen exam;
    int nota, idAl;
    for (int i = 0 to N-1) {
        Alumno[*]?entregar(idAl, exam);
        CorregirExamen(exam, nota);
        Alumno[idAl]!corregir(nota);
    }
}   