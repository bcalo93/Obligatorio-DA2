import { Component, OnInit } from '@angular/core';
import { Area } from 'src/models';
import { AreaService } from 'src/services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImporterIndicatorService } from 'src/services/importer-indicator.service';

const fakeResponse = [
  {
      name: 'XML Importer',
      parameters: [
          {
              name: 'File path',
              type: 'Text',
              keyName: 'filePath'
          },
          {
            name: 'Number of retries',
            type: 'Int',
            keyName: 'numberRetries'
          },
          {
            name: 'Excecute in date',
            type: 'Date',
            keyName: 'excecutedDate'
          }
      ]
  },

  {
    name: 'JSON Importer',
    parameters: [
        {
            name: 'File path',
            type: 'Text',
            keyName: ''
        },
        {
          name: 'Number of retries',
          type: 'Int',
          keyName: ''
        }
    ]
}
];


@Component({
  selector: 'app-importer',
  templateUrl: './importer.component.html',
  styleUrls: ['./importer.component.css']
})
export class ImporterComponent implements OnInit {

  allAreas: Array<Area> = [];
  availableImporters: Array<any> = [] ;
  selectedImporter = '';
  selectedAreaId = '';
  errorMessage = '';
  errorImporterMessage = '';
  successImporterMessage = '';

  importerParameters: Array<any> = [] ;

  myFormGroup = new FormGroup({});
  constructor(
    private areaService: AreaService,
    private importerService: ImporterIndicatorService) { }



  ngOnInit() {
    this.areaService.getAllAreas().subscribe(
      areas => this.allAreas = areas.reverse(),
      error => this.errorMessage = error
    );
    this.importerService.getImporterInfo().subscribe(
      response => {
        console.log(response);
        this.availableImporters = response;
      },
      error => this.errorMessage = error
    );

    console.log('selectedImporter', this.selectedImporter);
    console.log('selectedAreaId', this.selectedAreaId);

  }

  areaSelection() {
    console.log('selectedAreaId', this.selectedAreaId);
  }

  importerSelection(event: any) {
    console.log('event', event);
    this.selectedImporter = event;
    this.importerParameters = this.getActualImporter().parameters;
    let group = { };
    this.importerParameters.forEach(
      item => group[item.name] = new FormControl('', [Validators.required])
    );
    this.myFormGroup = new FormGroup(group);
    console.log(this.importerParameters);
  }

  getActualImporter() {
    return this.availableImporters.find(x => x.name === this.selectedImporter);
  }

  sumbit() {

    const actualImporter = this.getActualImporter();
    const parameters = {};

    actualImporter.parameters.forEach(
      item => {
        const keyName = item.keyName;
        const formValue = this.myFormGroup.controls[item.name].value;
        const value = (formValue instanceof Date) ? this.tranformDate(formValue) : formValue;
        parameters[keyName] = value;
    });

    const body = {
      areaId: this.selectedAreaId,
      importerName: actualImporter.name,
      parameters
    };
    console.log(body);
    console.log(JSON.stringify(body));
    this.importerService.importData(body).subscribe(
      response => {
        console.log('RESPONSE: ', response);
        if (response.error) {
          this.errorImporterMessage = response.error;
          this.successImporterMessage = '';
        } else {
          this.successImporterMessage =
            response.indicatorsImported + ' / ' +
            response.totalIndicators + ' INDICATORS IMPORTED';
          this.errorImporterMessage = '';
        }
      },
      error => {
        this.successImporterMessage = '';
        this.errorMessage = error;
      }

    );
  }

  tranformDate(date: any){

    return date.toISOString().split('T')[0];
  }

  isFormInvalid() {
    return (this.myFormGroup.invalid) ? true : false;
  }

  goBack() {
    this.selectedAreaId = '';
    this.selectedImporter = '';
    this.importerParameters = [];
  }

  isEmptySelection() {
    return this.selectedAreaId === '' || this.selectedImporter === '';
  }

  onChangeInt(event: any, control: any) {
    const value = parseInt(event.target.value);
    if (isNaN(value)) {
      this.myFormGroup.get(control).setErrors({incorrect: true});
    }
  }

  onChangeDate(event: any, control: any) {
    let value = event.target.value;
    if (value instanceof Date) {
      value = value.toISOString().split('T')[0];
    } else { this.myFormGroup.get(control).setErrors({incorrect: true}); }
  }
}
