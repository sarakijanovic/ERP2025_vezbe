import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WatercoolerUpdateDialogComponent } from './watercooler-update-dialog.component';

describe('WatercoolerUpdateDialogComponent', () => {
  let component: WatercoolerUpdateDialogComponent;
  let fixture: ComponentFixture<WatercoolerUpdateDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WatercoolerUpdateDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WatercoolerUpdateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
