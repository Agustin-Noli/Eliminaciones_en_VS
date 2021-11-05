import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImprubrosComponent } from './imprubros.component';

describe('ImprubrosComponent', () => {
  let component: ImprubrosComponent;
  let fixture: ComponentFixture<ImprubrosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImprubrosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImprubrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
