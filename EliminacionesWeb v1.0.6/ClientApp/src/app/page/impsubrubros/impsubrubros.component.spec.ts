import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImpsubrubrosComponent } from './impsubrubros.component';
//import { ImprubrosComponent } from './imprubros.component';
describe('ImpsubrubrosComponent', () => {
  let component: ImpsubrubrosComponent;
  let fixture: ComponentFixture<ImpsubrubrosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ImpsubrubrosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImpsubrubrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
