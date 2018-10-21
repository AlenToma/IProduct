class ShoppingCart extends React.Component {
	constructor(option) {
		super(option);
		this.state = {
			elem: undefined,
			get: option.get,
			save: option.save,
			field: option.field,
			image: option.image,
			connector: option.connector, // its a top menu shopping cart,
			display: option.display,
			itemCount: 0,
			data: undefined,
			isLoaded: false
		};
	}

	componentDidMount() {
		var x = this;
		$("body").ax({
			url: this.state.get,
			async: false,
			success: function (result) {
				x.setState(state => ({
					data: result
				}));
			}
		});
		this.componentDidUpdate();
	}

	componentDidUpdate() {
		var $this = $(ReactDOM.findDOMNode(this));
		if ($this)
			toLocation($($this).find(":not([href=''])"));
	}

	render() {
		var x = this;
		x.update = function () {
			x.componentDidMount();
		};

		// Add Product To Shopping cart
		x.add = function (productId, count) {
			if (!count)
				count = 1;
			$("body").ax({
				url: x.state.save,
				data: JSON.stringify({ productId: productId, count: count }),
				async: false,
				success: function (data) {
					x.componentDidMount();
				}
			});
		};

		// Remove Product From Shopping cart
		x.remove = function (productId) {
			$("body").dialog({
				title: "Please Confirm",
				data: $("<span class='info'></span>").html("Are you sure?"),
				onConfirm: function () {
					x.add(productId, -110000);
				}
			}).Show();
		};

		// Get the total product 
		x.count = function () {
			x.componentDidMount();
			return x.state.count;
		};

		// get shopping cart invoice, to much work needed for this yet
		x.invoice = function () {
			var result;
			$("body").ax({
				url: x.state.get,
				async: false,
				success: function (data) {
					result = data;
				}
			});

			return result;
		};

		var header;
		if (x.state.data === undefined)
			return (<div style={{ display: 'none' }} />);
		var total = 0;
		var totalSum = 0;
		$.each(x.state.data.productTotalInformations, function () {
			total += this.v;
		});
		x.state.count = total;

		if (x.state.connector) {
			if (x.state.data.products.length > 0)
				$(x.state.connector).html("(" + total + ")");
			else $(x.state.connector).html("");
		}

		if (this.state.display === "none")
			return (<div style={{ display: 'none' }} />);


		header = (
			<thead>
				<tr>
					<th>Product</th>
					<th>Price</th>
					<th>Quantity</th>
					<th className="text-center">Subtotal</th>
					<th />
				</tr>
			</thead>);

		var body = (
			<tbody>
				{this.state.data.products.map(d => {

					const url = x.state.image + '/Home/Product?id=' + d.id;
					const src = x.state.image + d.images[0].images.fileThumpFullPath;
					const description = isNullOrEmpty(d.description) ? "" : d.description;
					const info = $.grep(x.state.data.productTotalInformations, function (a) { return a.k === d.id })[0];
					totalSum += d.price * info.v;
					return (<tr key={d.id}>
						<td data-the="Product">
							<div className="row">
								<div className="col-sm-2">
									<img style={{ width: '100%' }} className="img-responsiv" href={url} src={src} />
								</div>
								<div className="col-sm-10 text-content">
									<h2 href={url} className="nomargin">{d.name}</h2>
									<p href={url}>{description}</p>
								</div>
							</div>
						</td>
						<td data-th="Price">{d.price.formatMoney()}:-</td>
						<td data-th="Quantity">
							<div className="row">
								<div className="col-sm-3">
									<div className="input-group">
										<span className="input-group-btn">
											<button type="button" className="btn btn-default btn-number" onClick={() => x.add(d.id, -1)} data-type="minus">
												<span className="glyphicon glyphicon-minus" />
											</button>
										</span>
										<input type="number" readOnly="true" style={{ width: '52px' }} className="form-control input-number" value={info.v} />
										<span className="input-group-btn">
											<button type="button" className="btn btn-default btn-number" onClick={() => x.add(d.id, 1)} data-type="plus">
												<span className="glyphicon glyphicon-plus" />
											</button>
										</span>
									</div>
								</div>
							</div>
						</td>
						<td data-th="Subtotal">{(d.price * info.v).formatMoney()}:-</td>
						<td data-th="action">
							<button onClick={() => x.remove(d.id)} className="btn btn-danger btn-sm">
								<i className="fa fa-trash-o" />
							</button>
						</td>
					</tr>);
				})}
			</tbody>);
		return (
			<div className="shoppingCart">
				<table id="cart" className="table table-hover table-condensed">
					{header}
					{body}
					<tfoot>
						<tr className="visible-xs">
							<td className="text-center">
								<strong>Total:{totalSum.formatMoney()}:-</strong>
							</td>
						</tr>
						<tr>
							<td>
								<a className="btn btn-warning">
									<i className="fa fa-angle-left" />
									Continue Shopping

								</a>
							</td>
							<td className="hidden-xs" />
							<td colSpan="2" style={{ textAlign: 'right' }} className="hidden-xs text-center">
								<strong>Total:{totalSum.formatMoney()}:-</strong>
							</td>
							<td>
								<a href="#" className="btn btn-success btn-block">
									Checkout
									<i className="fa fa-angle-right" />
								</a>
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		);
	}
}

//ReactDOM.render(<ShoppingList name="alen" />, document.getElementById('content')); 

//$("#content").html(ReactDOM.render(<ShoppingList name="alen" />));