import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WatercoolerInfoDialogComponent } from './watercooler-info-dialog.component';

describe('WatercoolerInfoDialogComponent', () => {
  let component: WatercoolerInfoDialogComponent;
  let fixture: ComponentFixture<WatercoolerInfoDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WatercoolerInfoDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WatercoolerInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
