using System;
using System.Collections;
using System.Collections.Generic;

namespace Zexil.DotNet.FlowAnalysis {
	/// <summary>
	/// Switch target list
	/// </summary>
	public sealed class TargetList : IList<BasicBlock> {
		private BasicBlock? _owner;
		private readonly List<BasicBlock> _targets;

		/// <summary>
		/// Owner of current instance
		/// </summary>
		public BasicBlock? Owner {
			get => _owner;
			internal set => _owner = value;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public TargetList() {
			_targets = new List<BasicBlock>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="capacity">Initial capacity</param>
		public TargetList(int capacity) {
			_targets = new List<BasicBlock>(capacity);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="targets">Targets</param>
		public TargetList(IEnumerable<BasicBlock> targets) {
			_targets = new List<BasicBlock>(targets ?? throw new ArgumentNullException(nameof(targets)));
		}

		private void UpdateReferences(BasicBlock? oldValue, BasicBlock? newValue) {
			if (_owner is null)
				throw new InvalidOperationException();
			_owner.UpdateReferences(oldValue, newValue);
		}

		#region implement
		/// <inheritdoc />
		public int Count => _targets.Count;

		/// <inheritdoc />
		public bool IsReadOnly => ((IList<BasicBlock>)_targets).IsReadOnly;

		/// <inheritdoc />
		public BasicBlock this[int index] {
			get => _targets[index];
			set {
				var oldValue = _targets[index];
				_targets[index] = value;
				UpdateReferences(oldValue, value);
			}
		}

		/// <inheritdoc />
		public int IndexOf(BasicBlock item) {
			return _targets.IndexOf(item);
		}

		/// <inheritdoc />
		public void Insert(int index, BasicBlock item) {
			_targets.Insert(index, item);
			UpdateReferences(null, item);
		}

		/// <inheritdoc />
		public void RemoveAt(int index) {
			var oldValue = _targets[index];
			_targets.RemoveAt(index);
			UpdateReferences(oldValue, null);
		}

		/// <inheritdoc />
		public void Add(BasicBlock item) {
			_targets.Add(item);
			UpdateReferences(null, item);
		}

		/// <inheritdoc />
		public void Clear() {
			foreach (var target in _targets)
				UpdateReferences(target, null);
			_targets.Clear();
		}

		/// <inheritdoc />
		public bool Contains(BasicBlock item) {
			return _targets.Contains(item);
		}

		/// <inheritdoc />
		public void CopyTo(BasicBlock[] array, int arrayIndex) {
			_targets.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(BasicBlock item) {
			if (_targets.Remove(item)) {
				UpdateReferences(item, null);
				return true;
			}
			else {
				return false;
			}
		}

		/// <inheritdoc />
		public IEnumerator<BasicBlock> GetEnumerator() {
			return ((IList<BasicBlock>)_targets).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IList<BasicBlock>)_targets).GetEnumerator();
		}
		#endregion
	}
}
