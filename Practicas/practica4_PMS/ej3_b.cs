/*
En un examen final hay N alumnos y P profesores. Cada alumno resuelve su examen, lo entrega y espera a que alguno de los profesores lo corrija y le indique la nota. Los profesores corrigen los exámenes respetando el orden en que los alumnos van entregando.
    b. Implemente una solución con PMS considerando que P>1.

Nota: maximizar la concurrencia y no generar demora innecesaria.
*/

Process Alumno[id:0.. N-1]{
    tipo_examen exam;
    int nota;
    ResolverExamen(exam); // asumir que existe
    MesaEntrega!entregar(id, exam);
    
    MesaEntrega?nota(nota);
}

Process MesaEntrega{
    int idAl, idP, nota;
    tipo_examen exam;
    cola examenes;
    while true {
        do Alumno[*]?entregar(idAl, exam) -->
            push(examenes, (idAl, exam));
        □ not empty(examenes); Profesor[*]?pedido(idP) --> 
            pop(examenes, (idAl, exam));
            Profesor[idP]!corregir(idAl, exam);
        □ Profesor[*]?correccion(idAl, nota) -->
            Alumno[idAl]!nota(nota);
    }
}

Process Profesor[id:0.. P-1]{
    int idAl, nota;
    tipo_examen exam;
    while true{
        MesaEntrega!pedido(id);
        MesaEntrega?corregir(idAl, exam);
        CorregirExamen(exam, nota); // assumir que existe
        MesaEntrega!correccion(idAl, nota);
    }
}   