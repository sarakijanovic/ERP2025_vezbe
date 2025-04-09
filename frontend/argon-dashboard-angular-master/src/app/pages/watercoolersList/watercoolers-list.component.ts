import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { WatercoolerDTO } from 'src/app/DTOs/watercoolerDTO';
import { WatercoolerUpdateDialogComponent } from 'src/app/components/dialogs/watercooler-update-dialog/watercooler-update-dialog.component';
import { Watercooler } from 'src/app/models/watercooler';
import { WatercoolerType } from 'src/app/models/watercoolerType';
import { WatercoolerService } from 'src/app/services/watercoolerService/watercooler.service';
import { WatercoolerTypeService } from 'src/app/services/watercoolerTypeService/watercooler-type.service';

@Component({
  selector: 'app-watercoolers-list',
  templateUrl: './watercoolers-list.component.html',
  styleUrls: ['./watercoolers-list.component.scss']
})
export class WatercoolersList implements OnInit {
  currentPage: number;
  disabledForward: boolean;
  watercoolers: WatercoolerDTO[];
  watercoolerInfo: WatercoolerDTO;
  watercoolerTypes: WatercoolerType[];
  role: string;
  
  constructor(private watercoolerService: WatercoolerService,
              private watercoolerTypeService: WatercoolerTypeService,
              private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.role = localStorage.getItem("uloga")
    this.startSubscription();
  }

  startSubscription() {
    this.watercoolerService.getAllWatercoolers(1, 4).subscribe(res => 
      {
        this.watercoolers = res;
        this.watercoolers.forEach(element => {
          this.watercoolerTypeService.getWatercoolerTypeById(element.tipAparataID).subscribe(result=> {
            element.tipAparataNaziv = result.nazivTipa
          })
        });
        this.currentPage = 1;
        this.getAllWatercoolerTypes();
      });
  }
  getAllWatercoolerTypes() {
    this.watercoolerTypeService.getAllWatercoolerTypes().subscribe(res => 
      {
        this.watercoolerTypes = res;
      });
  }

  deleteWatercooler(watercooler: WatercoolerDTO) {
    if(confirm("Are you sure you want to delete watercooler " + watercooler.model + "?")) {
      const watercoolerIndex = this.watercoolers.findIndex( (obj: WatercoolerDTO) => obj.aparatID == watercooler.aparatID);
      this.watercoolers.splice(watercoolerIndex, 1);
      this.watercoolerService.deleteWatercooler(watercooler.aparatID).subscribe();
    }
  }

  updateWatercooler(watercooler: WatercoolerDTO) {
    this.watercoolerInfo = watercooler;
    this.watercoolerTypeService.getWatercoolerTypeById(watercooler.tipAparataID).subscribe(res => 
    {
      this.watercoolerInfo.tipAparataNaziv = res.nazivTipa;
    })
    let dialog = this.dialog.open(WatercoolerUpdateDialogComponent, {
      width: '600px',
      data: {
        isEdit: true,
        watercooler: this.watercoolerInfo,
        watercoolerTypes: this.watercoolerTypes
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        result.aparatID = this.watercoolerInfo.aparatID
        this.watercoolerService.updateWatercooler(result).subscribe(res => {
          const watercoolerIndex = this.watercoolers.findIndex( (obj: WatercoolerDTO) => obj.aparatID == watercooler.aparatID);
          this.watercoolers[watercoolerIndex] = result;
          this.watercoolerTypes.forEach(element => {
            if(element.tipAparataID == result.tipAparataID) {
              this.watercoolers[watercoolerIndex].tipAparataNaziv = element.nazivTipa
            }
          });
        });
      }
    });
  }

  createWatercooler() {
    let dialog = this.dialog.open(WatercoolerUpdateDialogComponent, {
      width: '600px',
      data: {
        isEdit: false,
        watercooler: {
          model: '',
          proizvodjac: '',
          opis: '',
          slikaURL: '',
          cena: '',
          kolicinaNaStanju: '',
          tipAparataID: '',
          tipAparataNaziv: ''
        },
        watercoolerTypes: this.watercoolerTypes
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        this.watercoolerService.createWatercooler(result).subscribe(res => {
          this.watercoolers.push(result);
        });
      }
    });
  }

  changePage(page: number) {
    if (page >= 1) {
      this.watercoolerService.getAllWatercoolers(page, 4).subscribe(res => 
        {
          if(res != null) {
            this.currentPage = page;
            this.watercoolers = res;
            this.watercoolers.forEach(element => {
              this.watercoolerTypeService.getWatercoolerTypeById(element.tipAparataID).subscribe(result=> {
                element.tipAparataNaziv = result.nazivTipa
              })
            });
            if(this.watercoolers.length < 4)
              {
                this.disabledForward = true;
              } else {
                this.disabledForward = false;
              }
          } else {
            this.disabledForward = true;
          }
        });
    }
  }
}
