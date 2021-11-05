import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';
import { ProcesosService } from 'src/app/servicios/procesos.service'
@Injectable({
  providedIn: 'root'
})
@Injectable()
export class ReportesService {

 
    constructor(private router: Router, private procesosServ: ProcesosService) { 

  }






}


