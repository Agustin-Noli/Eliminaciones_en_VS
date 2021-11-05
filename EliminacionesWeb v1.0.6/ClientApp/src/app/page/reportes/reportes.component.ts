import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArrayName, FormArray } from '@angular/forms';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { AgGridAngular } from 'ag-grid-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import * as xJS from '../../../locale.en.js';
import { userAD } from 'src/app/Model/UserAD';
import * as $ from 'jquery';

@Component({
    selector: 'app-reportes',
    templateUrl: './reportes.component.html',
    styleUrls: ['./reportes.component.scss'],
    providers: [DatePipe]
})
export class ReportesComponent implements OnInit {
    rowSelection;
    defaultColDef;
    rowData: any;
    grupo; fecha; rfecha; fec;
    obj; grupos;
    date = new Date();
    public showtxt: boolean = false;
    public alerttext: boolean = false;
    public errortext: boolean = false;
    public showexcel: boolean = false;
    public alertexcel: boolean = false;
    public errorexcel: boolean = false;
    idgrupo; fecperiodoactual;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [
        { headerName: 'Cod Empresa', field: 'empCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Empresa', field: 'empDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Periodo', field: 'periodo', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Cod Contraparte', field: 'empCodigoContraparte', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Contraparte', field: 'empDescripcionContraparte', sortable: true, filter: true, resizable: true, suppressMovable: true },
      { headerName: 'Rubro', field: 'rubCodigo', sortable: true, resizable: true, filter: true, suppressMovable: true },
      { headerName: 'Moneda', field: 'monDescripcion', sortable: true, resizable: true, suppressMovable: true },
      { headerName: 'Saldo', field: 'eliSaldo', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value)  },
      { headerName: 'Saldo Promedio', field: 'eliSaldoPromedio', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value)  },
        { headerName: 'Eliminacion %', field: 'empPorcentaje', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Cod Moneda', field: 'monCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Sec Codigo', field: 'secCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Sec Descripcion', field: 'secDescripcion', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Es Ajuste', field: 'esAjuste', sortable: true, resizable: true, suppressMovable: true, hide: true }



    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    myform: FormGroup;
    reportes = [];

  constructor(private mf: FormBuilder, private procesosServ: ProcesosService, private route: ActivatedRoute, private datePipe: DatePipe, private router: Router) {
   
    var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
    
   if (UserAD == undefined || UserAD == null) {
     
    this.procesosServ.getacceso()
    UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
    if (UserAD == null) {
      this.router.navigate(['']);
      }
   }
        
    if (route.snapshot.url.length == 0) {
      
      this.procesosServ.logo = true;
    } else {
      
      this.procesosServ.logo = false;
    }
        this.fecha = this.datePipe.transform(this.date, 'yyyy-MM')
        //this.getgtupos();

   // if (this.procesosServ.data == undefined) {
    
      //this.router.navigate(['']);
      // this.procesosServ.getAd()
      //.subscribe((res) => {
      //  if (res != null) {

      //    this.procesosServ.data = res
      //    this.procesosServ.secCod = res.secCodigo

      //    console.log(res);
      //    debugger
      //  } else {
      //    this.router.navigate(['']);
      //  }
      //}, err => {
      //  console.log(err)
      //});
   // }
        this.myform = this.mf.group({
            fecha: [this.fecha, Validators.required],
            grupo: ["", Validators.required]


        })

        this.rowSelection = 'single';

        this.fecha = this.datePipe.transform(this.date, 'yyyy-MM')


    }

  ngOnInit() {
    this.periodoactual();
  }

    //getgtupos() {
    //    this.procesosServ.getGrupo()
    //        .subscribe((data) => {
    //            if (data != null) {
    //                this.grupos = data
    //                this.idgrupo = this.grupos[0].grupoId

    //                this.periodoactual();
    //            }
    //        }, err => {
    //            console.log(err);
    //        }
    //        );

    //}


    onSelectionChanged(e) {


        const selectedNodes = this.agGrid.api.getSelectedNodes();
        // const selectedData = selectedNodes.map( node => node.data );
        // const selectedDataStringPresentation = selectedData.map( node => node.make + ' ' + node.model).join(', ');
        this.obj = selectedNodes
        if (this.obj.length == 0) {
            this.obj = undefined;
        } else {
            this.obj = selectedNodes[0].data

        }

    }

    ShowTXT() {
        this.showtxt = true;
        this.alerttext = false;
        this.errortext = false;
    }

    generatxt() {
        this.procesosServ.posttxt()
            .subscribe(res => {
                if (res.mensaje == "OK") {
                    this.alerttext = true;
                }
                else {
                    this.errortext = true;
                }

            }, err => {
                console.log(err)
            }
            );
    }

  ShowExcel() {
        this.alerttext = false;
        this.errortext = false;
        this.showexcel = true;
        this.alertexcel = false;
        this.errorexcel = false;
    }

    generaExcel() {
        // Genero el txt automaticamente antes de crear el excel
        this.generatxt();

        var data = {
            "fecha": this.procesosServ.fecreport
        }

        this.procesosServ.GetGenerarExcel(data);
        this.showexcel = false;
    }

    periodo() {
        // mandar  grupoid
        this.fecha = this.myform.value.fecha
        this.fecperiodoactual = this.fecha.split(" ")[0].split("-").reverse().join("-");
        //  this.procesosServ.myFecha=  this.fecperiodoactual.replace("-", "")
        let datafecha = this.fecperiodoactual.replace("-", "")

        this.procesosServ.fecreport = datafecha
        //let grup = ((this.myform.value.grupo == "" || this.myform.value.grupo == null) ? this.idgrupo : this.myform.value.grupo);
        //let grup = "";


        //this.grupos.forEach(item => {
        //    if (this.myform.value.grupo == item.grupoNombre) {
        //        console.log(item.grupoId)
        //        grup = item.grupoId
        //        return
        //    }
        //});


        var data = {
            //"grupo": grup,
            "fecha": datafecha
        }
        this.procesosServ.GetEliminaciones(data)
            .subscribe((data) => {
                if (data != null) {
                    this.rowData = data
                }
                else {
                    this.rowData = ""
                }
            }, err => {
                console.log(err);
            }
            );
    }

    periodoactual() {

        this.fecha = this.datePipe.transform(this.date, 'yyyy-MM')
        this.rfecha = this.fecha.split(" ")[0].split("-").reverse().join("-");

        this.fec = this.rfecha.replace("-", "")

        this.procesosServ.fecreport = this.fec
        console.log(this.fecha)

        this.procesosServ.data

        var data = {
            /*"grupo": this.idgrupo,*/
            "fecha": this.procesosServ.fecreport
        }
        this.procesosServ.GetEliminaciones(data)
            .subscribe((data) => {
                if (data != null) {
                    this.rowData = data
                } else {
                    this.rowData = ""
                }

            }, err => {
                console.log(err);
            }
            );

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
