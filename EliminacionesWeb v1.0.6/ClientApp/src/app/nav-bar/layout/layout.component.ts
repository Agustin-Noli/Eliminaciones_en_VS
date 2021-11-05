import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProcesosService } from 'src/app/servicios/procesos.service'
@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
    public contable: boolean = false;
    public grupo: boolean = false;
    public logo: boolean = false;
    public usuario;
    public sector;
    public data;
    public secCodigo;
    public perfil;
    public datauser;
      public UserAD

    constructor(public procesosServ: ProcesosService, private route: ActivatedRoute) {

      this.UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
      
      
    }

    ngOnInit() {
      
      this.imagsector();
      
    }



    imagsector() {
        this.usuario   = this.UserAD.usuNombre
        this.sector    = this.UserAD.secDescripcion
        this.secCodigo = this.UserAD.secCodigo
        this.perfil = this.UserAD.usuPerfil

      this.procesosServ.secCod = this.UserAD.secCodigo

        if (this.secCodigo == 1) {
            this.grupo = true;
        } else {
            this.contable = true
        }
    }





}
