import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-imprubros',
    templateUrl: './imprubros.component.html',
    styleUrls: ['./imprubros.component.scss']
})
export class ImprubrosComponent implements OnInit {

    defaultColDef;
    rowSelection;
  permitido: boolean = false;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;

    title = 'app';

    columnDefs = [
        { headerName: 'Rubro', field: 'rubCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Descripcion', field: 'rubDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Signo', field: 'rubSigno', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Nivel', field: 'rubNivel', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Orden', field: 'rubOrden', sortable: true, resizable: false, suppressMovable: false },
        { headerName: 'Bcra', field: 'esBcra', sortable: true, resizable: true, suppressMovable: true, hide: true },

    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;

    rowData: any;

  constructor(private http: HttpClient, private procesosServ: ProcesosService, private route: ActivatedRoute,private router: Router) {

      var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

      if (UserAD == undefined || UserAD == null) {

        this.procesosServ.getacceso()
        UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
        if (UserAD == null) {
          this.router.navigate(['']);
        }
      }

    let permit = (UserAD.usuPerfil == "Operador") ? false : true;

    this.permitido = permit


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

        this.procesosServ.getRubros()
            .subscribe((data) => {
                if (data.length == 0) {
                    //console.log(this.rowData);
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

    }




}
