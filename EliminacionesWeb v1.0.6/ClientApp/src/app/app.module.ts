import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LayoutComponent } from './nav-bar/layout/layout.component';
import { ProcesosComponent } from './page/procesos/procesos.component';
import { MonedaComponent } from './page/moneda/moneda.component';
import { AjustesComponent } from './page/ajustes/ajustes.component';
import { ImprubrosComponent } from './page/imprubros/imprubros.component';
import { ReportesComponent } from './page/reportes/reportes.component';
import { EmpresasComponent } from './page/empresas/empresas.component';
import { ImpplanillasComponent } from './page/impplanillas/impplanillas.component';
import { UsuariosComponent } from './page/usuarios/usuarios.component';
import { GruposComponent } from './page/grupos/grupos.component';
import { AgGridModule } from 'ag-grid-angular';
import { AccesoComponent } from './login/acceso/acceso.component';
import { GrupoempresaComponent } from './page/grupoempresa/grupoempresa.component';
import { HistorialComponent } from './page/historial/historial.component';
import { ImpsubrubrosComponent } from './page/impsubrubros/impsubrubros.component'
import { RelacionrubrosComponent } from './page/relacionrubros/relacionrubros.component'
//import { NgSelectModule, NgOption } from '@ng-select/ng-select';
import { NgSelectModule} from '@ng-select/ng-select';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap'; 
import { NgSelectConfig } from '@ng-select/ng-select';
import { ɵs } from '@ng-select/ng-select';
import * as $ from 'jquery';

@NgModule({
    declarations: [
        AppComponent,
        AccesoComponent,
        LayoutComponent,
        ProcesosComponent,
        MonedaComponent,
        AjustesComponent,
        ImprubrosComponent,
        ReportesComponent,
        EmpresasComponent,
        ImpplanillasComponent,
        UsuariosComponent,
        GruposComponent,
        AccesoComponent,
        GrupoempresaComponent,
        HistorialComponent,
        ImpsubrubrosComponent,
        RelacionrubrosComponent, 
    ], 
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule,
        //NgbModule,
        AgGridModule.withComponents([]),
    ],
    providers: [NgSelectConfig, ɵs],
    bootstrap: [AppComponent]
})
export class AppModule { }

