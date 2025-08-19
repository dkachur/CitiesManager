import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editCityID: string | null = null;

  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      name: new FormControl(null, [Validators.required])
    });

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    })
  }

  get putCityFormArray(): FormArray {
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities()
      .subscribe({
        next: (res: City[]) => {
          this.cities = res;

          this.cities.forEach(city => {
            this.putCityFormArray.push(new FormGroup({
              id: new FormControl(city.id, [Validators.required]),
              name: new FormControl({value: city.name, disabled: true}, [Validators.required])
            }));
          });
        },

        error: (error: any) => {
          console.log(error);
        },

        complete: () => { },
      });
  }

  ngOnInit() {
    this.loadCities();
  }

  get cityNameFromPostControl(): any {
    return this.postCityForm.controls['name'];
  }

  public postCitySubmitted() {
    this.isPostCityFormSubmitted = true;

    console.log(this.postCityForm.value);

    this.citiesService.postCity(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response);

        //this.loadCities();
        this.putCityFormArray.push(new FormGroup({
          id: new FormControl(response.id, [Validators.required]),
          name: new FormControl({ value: response.name, disabled: true }, [Validators.required])
        }));

        this.cities.push({ id: response.id, name: response.name });

        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },
      error: (error: any) => { console.log(error) },
      complete: () => { }
    });
  }

  editClicked(city: City): void {
    this.editCityID = city.id;
  }

  updateClicked(i: number): void {
    this.citiesService.putCity(this.putCityFormArray.controls[i].value).subscribe(
      {
        next: (response: string) => {
          console.log(response);

          this.editCityID = null;

          this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value)
        },
        error: (err: any) => {
          console.log(err);
        },
        complete: () => { }
      });
  }

  deleteClicked(city: City, i: number): void {
    if (confirm(`Are you sure to delete this city ${city.name}?`)) {
      this.citiesService.deleteCity(city.id).subscribe({
        next: (response: string) => {
          console.log(response);

          this.putCityFormArray.removeAt(i);
          this.cities.splice(i, 1);
        },
        error: (err: any) =>
        {
          console.log(err);
        },
        complete: () => { }
      });
    }
  }
}
