import { Component } from '@angular/core';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

export class AppComponent {
    public showlayout: boolean = false;
    public showacceso: boolean = false;
    public user

    constructor(private procesosServ: ProcesosService, private router: Router, private route: ActivatedRoute) {

        // this.showlayout=true
        //  this.procesosServ.showlayout=true
        //this.showlayout=true
        if (route.snapshot.url.length == 0) {

            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }
        //METODO PARA USUARIOS AD

        var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

        if (UserAD == undefined) {
            this.procesosServ.getacceso()
            UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
            if (UserAD == null) {
                this.showlayout = false
                this.showacceso = true
            } else {
                this.showlayout = true
                this.showacceso = false

            }
        } else {
            this.showlayout = true
            this.showacceso = false
        }




    }

    title = 'FrontEliminaciones';
}
