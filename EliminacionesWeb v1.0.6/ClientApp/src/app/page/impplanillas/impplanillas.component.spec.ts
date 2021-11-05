import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImpplanillasComponent } from './impplanillas.component';

describe('ImpplanillasComponent', () => {
  let component: ImpplanillasComponent;
  let fixture: ComponentFixture<ImpplanillasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImpplanillasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImpplanillasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
