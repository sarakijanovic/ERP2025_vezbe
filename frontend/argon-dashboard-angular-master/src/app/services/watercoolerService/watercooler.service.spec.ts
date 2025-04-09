import { TestBed } from '@angular/core/testing';

import { WatercoolerService } from './watercooler.service';

describe('WatercoolerService', () => {
  let service: WatercoolerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WatercoolerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
