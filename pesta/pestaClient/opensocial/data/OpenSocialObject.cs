/* Copyright (c) 2008 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */



/**
 * An object representing a generic, extensible OpenSocial entity. Virtually
 * every object, both concrete (person) or abstract (data), having arbitrary
 * properties is modeled as an OpenSocialObject instance. Instance methods
 * provide an interface for associating strings with objects representing
 * properties or attributes of that object. These fields can in turn
 * reference other OpenSocialObject instances.
 *
 * @author Jason Cooper
 */
using System;
using System.Collections.Generic;

public class OpenSocialObject {

  protected Dictionary<String,OpenSocialField> fields;

  public OpenSocialObject() {
      this.fields = new Dictionary<String, OpenSocialField>();
  }

  /**
   * Instantiates a new OpenSocialObject object with the passed Map of
   * OpenSocialField objects keyed on strings, replicating these
   * correspondences in its own fields Map.
   * 
   * @param  properties Map of OpenSocialField objects keyed on field name
   *         which should be "imported" upon instantiation
   */
  public OpenSocialObject(Dictionary<String,OpenSocialField> properties) :this(){

    foreach(var e in properties) {
      this.setField(e.Key, e.Value);
    }
  }

  /**
   * Returns {@code true} if a field with the passed key is associated with
   * the current instance, {@code false} otherwise.
   */
  public bool hasField(String key) {
    return this.fields.ContainsKey(key);
  }

  /**
   * Returns field mapped to the passed key.
   * 
   * @param  key Key associated with desired field
   */
  public OpenSocialField getField(String key) {
    return this.fields[key];
  }

  /**
   * Creates a new entry in fields Map, associating the passed OpenSocialField
   * object with the passed key.
   * 
   * @param  key Field name
   * @param  value OpenSocialField object to associate with key
   */
  public void setField(String key, OpenSocialField value) {
    this.fields.Add(key, value);
  }

  /**
   * Returns the names of all properties associated with the instance as an
   * array of Java String objects.
   */
  public String[] fieldNames() {
    var keys = this.fields.Keys;
    String[] fieldNames = new String[this.fields.Count];
      int i = 0;
      foreach (var key in keys)
    {
      fieldNames[i++] = key;
    }

    return fieldNames;
  }
}
