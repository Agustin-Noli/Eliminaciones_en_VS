import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GrupoempresaComponent } from './grupoempresa.component';

describe('GrupoempresaComponent', () => {
  let component: GrupoempresaComponent;
  let fixture: ComponentFixture<GrupoempresaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GrupoempresaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GrupoempresaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
