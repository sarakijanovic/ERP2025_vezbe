import { TestBed } from '@angular/core/testing';

import { WatercoolerTypeService } from './watercooler-type.service';

describe('WatercoolerTypeService', () => {
  let service: WatercoolerTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WatercoolerTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
