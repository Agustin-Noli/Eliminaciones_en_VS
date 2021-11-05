import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute,Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-moneda',
    templateUrl: './moneda.component.html',
    styleUrls: ['./moneda.component.scss']
})
export class MonedaComponent implements OnInit {
    defaultColDef;
    rowSelection;
    showModal: boolean = false;
    showModeleliminar: boolean = false;
    showModelmodificar: boolean = false;
    select;
    id;
    porc;
    obj;
    porcenjate;
    descripcion;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;

    title = 'app';
    // 
    columnDefs = [
        { headerName: 'Codigo', field: 'monCodigo', sortable: true, filter: true, hide: true },
        { headerName: 'Moneda', field: 'monDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Cotizacion', field: 'monCotizacion', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value)   }
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
        this.procesosServ.getMoneda()
            .subscribe((data) => {
                if (data.length == 0) {
                } else {
                    this.rowData = data
                }
            }, err => {

                console.log("error")
                //  if(err.status=404){
                //    this.rowData =""
            }
            );

    }
    onSelectionChanged(e) {
        const selectedNodes = this.agGrid.api.getSelectedNodes();
        this.obj = selectedNodes[0].data
    }






}
function numberToPointedString(number) {
  if (number == null) {
    return number;
  } else {
    let result = number.toLocaleString('es-ar', {
      style: 'currency',
      currency: 'ARS',
      minimumFractionDigits: 2
    });

    return result;
  }


}
