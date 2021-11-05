import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, FormArrayName, FormArray } from '@angular/forms';
import { AgGridAngular } from 'ag-grid-angular';
import { RowNode } from 'ag-grid-community';
import { ProcesosService } from 'src/app/servicios/procesos.service';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';
import { async } from 'rxjs/internal/scheduler/async';

@Component({
    selector: 'app-impplanillas',
    templateUrl: './impplanillas.component.html',
    styleUrls: ['./impplanillas.component.scss'],
    providers: [DatePipe]
})
export class ImpplanillasComponent implements OnInit {
    defaultColDef;
    rowSelection;
    date = new Date();
    fecha;
    fecperiodoactual;
    rfecha; fec;
    obj; grupos;
    grupo;
    idgrupo;

    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;

    title = 'app';

    columnDefs = [
        { headerName: 'Codigo', field: 'empCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Empresa', field: 'empDescripcion', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Periodo', field: 'periodo', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Nombre Planilla', field: 'nombrePlanilla', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Porcentaje', field: 'porcentaje', sortable: true, resizable: true, suppressMovable: true, },
        { headerName: 'Sec Codigo', field: 'secCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        {
            headerName: 'Importacion', field: 'importacion', sortable: true, filter: true, resizable: true, suppressMovable: true, valueFormatter: x => (x.value == 'S') ? 'OK' : 'No Importada',
            cellStyle: x => (compareValues(x.value))
        }

    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    rowData: any;
    myform: FormGroup;

    constructor(private mf: FormBuilder, private http: HttpClient, private procesosServ: ProcesosService, private datePipe: DatePipe, private route: ActivatedRoute, private router: Router) {

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
        this.fecha = this.datePipe.transform(this.date, 'yyyy-MM')
        this.getgrupos();

        this.myform = this.mf.group({
            fecha: [this.fecha, Validators.required],
            grupo: ["", Validators.required]
        })
    }

    ngOnInit() {
        //this.SetearParametros();
        //this.GetGrillaPlanillasCargadas();
    }


    ChangeCombos() {
        this.SetearParametros();
        this.GetGrillaPlanillasCargadas();
    }


    GetGrillaPlanillasCargadas() {
        this.procesosServ.getPlanillasCargadas()
            .subscribe((data) => {
                if (data != null) {
                    this.rowData = data
                } else {
                    this.rowData = ""
                }
            }, err => {
                if (err.status = 404) {
                    this.rowData = ""
                }
            }
            );
    }

    getgrupos() {
        this.procesosServ.getGrupo()
            .subscribe((data: any) => {
                if (data != null) {
                    this.grupos = data
                    this.idgrupo = this.grupos[0].grupoId
                    // Carga Inicial
                    this.SetearParametros();
                    this.GetGrillaPlanillasCargadas();
                }
            }, err => {
                console.log(err);
            }
            )
    }

    SetearParametros() {
        this.fecha = this.myform.value.fecha;
        this.fecperiodoactual = this.fecha.split(" ")[0].split("-").reverse().join("-");
        this.procesosServ.myFechaimp = this.fecperiodoactual.replace("-", "")

        let grup = ((this.myform.value.grupo == null || this.myform.value.grupo == "") ? this.idgrupo : this.myform.value.grupo);

        this.grupos.forEach(item => {
            if (this.myform.value.grupo == item.grupoNombre) {
                grup = item.grupoId
                return
            }
        });
        this.procesosServ.myGrupoIdimp = grup;
    }

}


function compareValues(params) {
    if (params == 'S') {
        return { color: 'green', /*"font-weight": "bold"*/ };
    }
    else {
        return { color: 'red',/* "font-weight": "bold"*/ };
    }
}

