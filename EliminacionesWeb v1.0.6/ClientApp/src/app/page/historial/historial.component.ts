import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';
import { resource } from 'selenium-webdriver/http';

@Component({
    selector: 'app-historial',
    templateUrl: './historial.component.html',
    styleUrls: ['./historial.component.scss']



})


export class HistorialComponent implements OnInit {

    defaultColDef;
    rowSelection;

    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;

    title = 'app';

    columnDefs = [
        { headerName: 'HistId', field: 'histId', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Legajo', field: 'usuLegajo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Nombre', field: 'usuNombre', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Fecha Registro', field: 'fechaHora', sortable: true, filter: true, valueFormatter: x => dateFormatter(x.value), resizable: true, suppressMovable: true },
        { headerName: 'Accion', field: 'accion', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Mensaje', field: 'mensaje', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'SecCodigo', field: 'secCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Sector', field: 'secDescripcion', sortable: true, resizable: true, suppressMovable: true },

    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;

    rowData: any;

    constructor(private http: HttpClient, private procesosServ: ProcesosService, private route: ActivatedRoute, private router: Router) {

        var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

        if (UserAD == undefined || UserAD == null) {

            this.procesosServ.getacceso()
            UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
            if (UserAD == null) {
                this.router.navigate(['']);
            }
        }
        if (route.snapshot.url[0] == undefined) {
            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }
        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };

        this.rowSelection = 'single';
    }


    ngOnInit() {

        this.procesosServ.getHistorial()
            .subscribe((data) => {
                if (data.length == 0) {
                } else {
                    this.rowData = data
                }
            }, err => {

                console.log("error")
            }
            );

    }


    onSelectionChanged(e) {
        const selectedNodes = this.agGrid.api.getSelectedNodes();

    }


}

function dateFormatter(fecha) {
    //2020-11-04T22:37:01.73
    // setear lenguaje regional region es-AR
    navigator.language.valueOf["es-AR"]; 
    var fechaFull = new Date(Date.parse(fecha));

    var day = fechaFull.getDate();
    var month = fechaFull.getMonth() + 1;
    var year = fechaFull.getFullYear();

    var hour = fechaFull.getHours();
    var minutes = fechaFull.getMinutes();

    return day + '/' + month + '/' + year + '  ' + hour + ':' + minutes;
}
