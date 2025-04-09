import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatercoolersList } from './watercoolers-list.component';

describe('watercoolers-list', () => {
  let component: WatercoolersList;
  let fixture: ComponentFixture<WatercoolersList>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatercoolersList ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatercoolersList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
