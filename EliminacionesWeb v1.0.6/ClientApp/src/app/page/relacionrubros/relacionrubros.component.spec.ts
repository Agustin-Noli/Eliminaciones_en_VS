import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelacionrubrosComponent } from './relacionrubros.component';

describe('GrupoempresaComponent', () => {
  let component: RelacionrubrosComponent;
  let fixture: ComponentFixture<RelacionrubrosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RelacionrubrosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelacionrubrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
